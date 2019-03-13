using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TunnelQuest.Core;
using TunnelQuest.Core.Migrations.Data;

namespace TunnelQuest.DatabaseBuilder
{
    public static class WikiScraper
    {
        public static readonly string QUERY_URL = "http://wiki.project1999.com/api.php?action=query&generator=categorymembers&gcmlimit=max&format=json&gcmtitle=Category:";


        public static readonly string DETAIL_URL = "http://wiki.project1999.com/";

        public static readonly string NON_P99_CONTENT_FILE_NAME = "non_p99_content.json";
        public static readonly string NON_P99_CONTENT_CORRECTIONS_FILE_NAME = "non_p99_content_corrections.json";

        public static readonly string ITEMS_FILE_NAME = "items.json";
        public static readonly string ITEMS_CORRECTIONS_FILE_NAME = "items_corrections.json";

        public static readonly string SPELLS_FILE_NAME = "spells.json";
        public static readonly string SPELLS_CORRECTIONS_FILE_NAME = "spells_corrections.json";

        public static readonly string IMAGE_WRAPPER_FOLDER_NAME = "wiki.project1999.com_scrape";

        
        public static ScrapeResults Scrape(string dataOutputPath, string imageOutputPath, bool pauseOnError)
        {
            var startTime = DateTime.Now;
            var results = new ScrapeResults();

            ScrapeNonP99Content(dataOutputPath, pauseOnError);
            results.Include(ScrapeItems(dataOutputPath, imageOutputPath, pauseOnError));
            results.Include(ScrapeSpells(dataOutputPath, imageOutputPath, pauseOnError));
            

            Console.WriteLine("Finished in " + (DateTime.Now - startTime).TotalMinutes.ToString() + " minutes.  " + results.TotalDownloaded.ToString() + " items downloaded, " + results.TotalSkipped.ToString() + " items skipped, " + results.TotalImagesDownloaded.ToString() + " images downloaded, " + results.TotalImagesSkipped.ToString() + " images skipped.");

            return results;
        }

        public static void ScrapeNonP99Content(string dataOutputPath, bool pauseOnError)
        {
            var file = new WikiFileHelper<WikiData>(getFilePath(dataOutputPath, NON_P99_CONTENT_FILE_NAME), getCorrectionsFilePath(dataOutputPath, NON_P99_CONTENT_CORRECTIONS_FILE_NAME));
            Console.WriteLine("Searching " + file.FilePath + " for missing non-P99 data to pull from the wiki:");
            if (file.IsEmpty())
            {
                Console.WriteLine("Rebuilding " + file.FilePath + " from the wiki:");

                var list = new List<WikiData>();
                var nonP99Objects = queryWiki("Non-P99_Content");
                foreach (var wikiPage in nonP99Objects)
                {
                    list.Add(new WikiData()
                    {
                        WikiPageId = wikiPage.WikiPageId,
                        Name = wikiPage.Name
                    });
                }

                file.WriteToFile(list);
            }
        }

        public static ScrapeResults ScrapeItems(string dataOutputPath, string imageOutputPath, bool pauseOnError)
        {
            var startTime = DateTime.Now;
            var results = new ScrapeResults();

            var nonP99File = new WikiFileHelper<WikiData>(getFilePath(dataOutputPath, NON_P99_CONTENT_FILE_NAME), getCorrectionsFilePath(dataOutputPath, NON_P99_CONTENT_CORRECTIONS_FILE_NAME));
            var itemFile = new WikiFileHelper<WikiItemData>(getFilePath(dataOutputPath, ITEMS_FILE_NAME), getCorrectionsFilePath(dataOutputPath, ITEMS_CORRECTIONS_FILE_NAME));

            Console.WriteLine("Searching " + itemFile.FilePath + " for missing item data or images to pull from the wiki:");

            if (itemFile.IsEmpty())
            {
                Console.WriteLine("Rebuilding " + itemFile.FilePath + " from the wiki:");

                var newItemList = new List<WikiItemData>();
                var wikiItems = queryWiki("Items");
                var filteredItems = getOnlyP99Objects(wikiItems, nonP99File.ReadFromFile());
                foreach (var wikiPage in filteredItems)
                {
                    newItemList.Add(new WikiItemData()
                    {
                        WikiPageId = wikiPage.WikiPageId,
                        Name = wikiPage.Name
                    });
                }

                itemFile.WriteToFile(newItemList);
            }

            var itemList = itemFile.ReadFromFile(); // re-pull even if we just built it, to apply the corrections
            using (var webClient = new WebClient())
            {
                foreach (WikiItemData nextItem in itemList.Where(item => String.IsNullOrWhiteSpace(item.Name) == false))
                {
                    try
                    {
                        // download the wiki entry for the item, if necessary

                        if (String.IsNullOrWhiteSpace(nextItem.WikiPageId) == false && (String.IsNullOrWhiteSpace(nextItem.IconFileName) || nextItem.Stats == null || nextItem.Stats.Length == 0))
                        {
                            string itemHtml = webClient.DownloadString(Path.Join(DETAIL_URL, nextItem.Name));

                            var itemWikiDoc = new HtmlDocument();
                            itemWikiDoc.LoadHtml(itemHtml);

                            var dataElement = itemWikiDoc.DocumentNode.Descendants("div").Where(d => d.Attributes["class"]?.Value == "itemdata").FirstOrDefault();
                            if (dataElement == null)
                                throw new Exception("Couldn't find dataElement");

                            // set itemData.Stats

                            var statsElement = dataElement.Descendants("p").FirstOrDefault();
                            if (statsElement == null)
                                throw new Exception("Couldn't find statsElement");

                            if (nextItem.Stats.Length == 0)
                            {
                                var itemStats = new List<string>();
                                foreach (var childNode in statsElement.ChildNodes)
                                {
                                    string childNodeText = childNode.InnerText.Replace("\r", "").Replace("\n", "").Trim();

                                    if (childNode is HtmlTextNode && !String.IsNullOrWhiteSpace(childNodeText))
                                    {
                                        if (childNode.PreviousSibling != null && childNode.PreviousSibling.Name.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                                            itemStats[itemStats.Count - 1] += " " + childNodeText;
                                        else
                                            itemStats.Add(childNodeText);
                                    }
                                    else if (childNode.Name.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        itemStats[itemStats.Count - 1] += " " + childNodeText;
                                    }
                                }
                                nextItem.Stats = itemStats.ToArray();
                            }

                            // set itemData.IconFileName
                            if (String.IsNullOrWhiteSpace(nextItem.IconFileName))
                            {
                                var imageElement = dataElement.Descendants("img").FirstOrDefault();
                                if (imageElement == null)
                                    throw new Exception("Couldn't find imageElement");
                                string imageSrc = imageElement.Attributes["src"].Value;
                                if (imageSrc.StartsWith('/'))
                                    imageSrc = imageSrc.Remove(0, 1);
                                nextItem.IconFileName = imageSrc.Split('/')[1];
                            }

                            // re-write the data file to disk after every single item
                            itemFile.WriteToFile(itemList);

                            Console.WriteLine("[" + nextItem.WikiPageId + "] " + nextItem.Name);
                            results.TotalDownloaded++;
                            Thread.Sleep(250);
                        }
                        else
                            results.TotalSkipped++;


                        // download the image file, if necessary

                        string imageUrl = Path.Join(DETAIL_URL + "images/", nextItem.IconFileName);
                        string imageFilePath = Path.GetFullPath(Path.Join(Path.Join(imageOutputPath, IMAGE_WRAPPER_FOLDER_NAME), nextItem.IconFileName));

                        if (!File.Exists(imageFilePath))
                        {
                            var imageFileInfo = new FileInfo(imageFilePath);

                            if (!imageFileInfo.Directory.Exists)
                                Directory.CreateDirectory(imageFileInfo.Directory.FullName);

                            webClient.DownloadFile(imageUrl, imageFilePath);
                            results.TotalImagesDownloaded++;
                        }
                        else
                            results.TotalImagesSkipped++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error pulling " + nextItem.Name);
                        Console.WriteLine(ex.ToString());
                        if (pauseOnError)
                        {
                            Console.WriteLine("Press any key to continue building items");
                            Console.ReadKey();
                        }
                    } // end catch
                } // end foreach (itemListLine)
            } // end using (webClient)
            
            Console.WriteLine("Finished in " + (DateTime.Now - startTime).TotalMinutes.ToString() + " minutes.  " + results.TotalDownloaded.ToString() + " items downloaded, " + results.TotalSkipped.ToString() + " items skipped, " + results.TotalImagesDownloaded.ToString() + " images downloaded, " + results.TotalImagesSkipped.ToString() + " images skipped.");

            return results;
        }

        public static ScrapeResults ScrapeSpells(string dataOutputPath, string imageOutputPath, bool pauseOnError, IEnumerable<WikiQueryResult> nonP99Objects = null)
        {
            var startTime = DateTime.Now;
            var results = new ScrapeResults();
            
            var spellFile = new WikiFileHelper<WikiSpellData>(getFilePath(dataOutputPath, SPELLS_FILE_NAME), getCorrectionsFilePath(dataOutputPath, SPELLS_CORRECTIONS_FILE_NAME));
            var nonP99File = new WikiFileHelper<WikiData>(getFilePath(dataOutputPath, NON_P99_CONTENT_FILE_NAME), getCorrectionsFilePath(dataOutputPath, NON_P99_CONTENT_CORRECTIONS_FILE_NAME));

            Console.WriteLine("Searching " + spellFile.FilePath + " for missing spell data or images to pull from the wiki:");

            if (spellFile.IsEmpty())
            {
                Console.WriteLine("Rebuilding " + spellFile.FilePath + " from the wiki:");

                var newSpellList = new List<WikiSpellData>();
                var wikiSpells = queryWiki("Spells");
                var filteredSpells = getOnlyP99Objects(wikiSpells, nonP99File.ReadFromFile());
                foreach (var wikiPage in filteredSpells)
                {
                    newSpellList.Add(new WikiSpellData()
                    {
                        WikiPageId = wikiPage.WikiPageId,
                        Name = wikiPage.Name
                    });
                }

                spellFile.WriteToFile(newSpellList);
            }

            var spellList = spellFile.ReadFromFile(); // re-pull even if we just built it, to apply the corrections
            using (var webClient = new WebClient())
            {
                foreach (WikiSpellData nextSpell in spellList.Where(spell => String.IsNullOrWhiteSpace(spell.Name) == false))
                {
                    try
                    {
                        // download the wiki entry for the spell, if necessary

                        if (String.IsNullOrWhiteSpace(nextSpell.WikiPageId) == false && (String.IsNullOrWhiteSpace(nextSpell.IconFileName) || nextSpell.Description == null || nextSpell.EffectDetails.Count() == 0))
                        {
                            string spellHtml = webClient.DownloadString(Path.Join(DETAIL_URL, nextSpell.Name));

                            var wikiDoc = new HtmlDocument();
                            wikiDoc.LoadHtml(spellHtml);

                            // set nextSpell.IconFileName
                            var imageElement = wikiDoc.DocumentNode.Descendants("img").Where(d => d.Attributes["class"]?.Value == "thumbborder").FirstOrDefault();
                            if (imageElement != null)
                            {
                                if (String.IsNullOrWhiteSpace(nextSpell.IconFileName))
                                {
                                    string imageSrc = imageElement.Attributes["src"].Value;
                                    if (imageSrc.StartsWith('/'))
                                        imageSrc = imageSrc.Remove(0, 1);
                                    nextSpell.IconFileName = imageSrc.Split('/')[1];
                                }

                                // set nextSpell.Description
                                if (String.IsNullOrWhiteSpace(nextSpell.Description))
                                {
                                    var descElement = imageElement.ParentNode?.NextSibling?.NextSibling;
                                    if (descElement == null)
                                        throw new Exception("Couldn't find descElement");
                                    nextSpell.Description = descElement.InnerText.Trim();
                                }
                            }

                            // build list of class/level requirements
                            if (nextSpell.Requirements.Length == 0)
                            {
                                var classCodes = new List<string>();
                                var spellRequirements = new List<WikiSpellData.WikiSpellDataRequirements>();
                                var classesUlElement = wikiDoc.DocumentNode.Descendants("span").Where(span => span.Id == "Classes").FirstOrDefault()?.ParentNode?.NextSibling?.NextSibling;
                                if (classesUlElement != null && classesUlElement.Name == "ul")
                                {
                                    foreach (var classLinkElem in classesUlElement.Descendants("a"))
                                    {
                                        // ignore links that are for other things
                                        if (classLinkElem.GetAttributeValue("rel", null) == "nofollow")
                                            continue;

                                        string wikiClassName = classLinkElem.InnerText.Trim();

                                        // fix common wiki typos
                                        if (wikiClassName.Equals("ShadowKnight", StringComparison.InvariantCultureIgnoreCase))
                                            wikiClassName = ClassNames.ShadowKnight;

                                        string className = ClassNames.All.Where(cleanName => cleanName.Equals(wikiClassName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                        if (className == null)
                                            throw new Exception("Invalid class name " + className);

                                        string levelString = classLinkElem.NextSibling.InnerText.Replace('-', ' ').Trim().Split(' ')[1];
                                        int levelInt;
                                        if (!Int32.TryParse(levelString, out levelInt))
                                            throw new Exception("Non-integer level value " + levelString);

                                        spellRequirements.Add(new WikiSpellData.WikiSpellDataRequirements()
                                        {
                                            ClassCode = ClassCodes.GetCode(className),
                                            Level = levelInt
                                        });
                                    }
                                }
                                nextSpell.Requirements = spellRequirements.ToArray();
                            }

                            // build list of spell effect descriptions
                            if (nextSpell.EffectDetails.Length == 0)
                            {
                                var spellDetails = new List<string>();
                                var spellInfoLabelElems = wikiDoc.DocumentNode.Descendants("td").Where(td => td.Attributes["bgcolor"]?.Value == "#cedff2");
                                foreach (var labelElem in spellInfoLabelElems)
                                {
                                    // Spell effect lines on the wiki page are labeled "1:", "2:", etc.  Those
                                    // are the only ones we're interested in to build the effect descriptions
                                    string labelText = labelElem.Descendants("b").First().InnerText.Split('&')[0].Trim();
                                    int temp;
                                    if (Int32.TryParse(labelText, out temp))
                                    {
                                        string detailLine = labelElem.NextSibling.NextSibling.InnerText.Trim();
                                        spellDetails.Add(detailLine);
                                    }
                                }
                                nextSpell.EffectDetails = spellDetails.ToArray();
                            }

                            // build list of where to obtain
                            if (nextSpell.Sources.Length == 0)
                            {
                                var spellSources = new List<string>();
                                var whereToObtainElem = wikiDoc.DocumentNode.Descendants("span").Where(span => span.Id.Equals("Where_to_Obtain", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()?.ParentNode?.NextSibling?.NextSibling;
                                if (whereToObtainElem != null)
                                {
                                    if (whereToObtainElem.Name == "table")
                                        spellSources.Add("Purchased from NPC vendors");
                                    else
                                        spellSources.AddRange(whereToObtainElem.Descendants("li").Select(elem => elem.InnerText.Trim()));
                                }
                                nextSpell.Sources = spellSources.ToArray();

                                // re-write the data file to disk after every single spell
                                spellFile.WriteToFile(spellList);

                                Console.WriteLine("[" + nextSpell.WikiPageId + "] " + nextSpell.Name);
                                results.TotalDownloaded++;
                                Thread.Sleep(250);
                            }
                        }
                        else
                            results.TotalSkipped++;

                        // download the image file, if necessary
                        if (!String.IsNullOrWhiteSpace(nextSpell.IconFileName))
                        {
                            string imageUrl = Path.Join(DETAIL_URL + "images/", nextSpell.IconFileName);
                            string imageFilePath = Path.GetFullPath(Path.Join(Path.Join(imageOutputPath, IMAGE_WRAPPER_FOLDER_NAME), nextSpell.IconFileName));

                            if (!File.Exists(imageFilePath))
                            {
                                var imageFileInfo = new FileInfo(imageFilePath);

                                if (!imageFileInfo.Directory.Exists)
                                    Directory.CreateDirectory(imageFileInfo.Directory.FullName);

                                webClient.DownloadFile(imageUrl, imageFilePath);
                                results.TotalImagesDownloaded++;
                            }
                            else
                                results.TotalImagesSkipped++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error pulling " + nextSpell.Name);
                        Console.WriteLine(ex.ToString());
                        if (pauseOnError)
                        {
                            Console.WriteLine("Press any key to continue building spells");
                            Console.ReadKey();
                        }
                    } // end catch
                } // end foreach
            } // end using (webClient)
            
            Console.WriteLine("Finished in " + (DateTime.Now - startTime).TotalMinutes.ToString() + " minutes.  " + results.TotalDownloaded.ToString() + " items downloaded, " + results.TotalSkipped.ToString() + " items skipped, " + results.TotalImagesDownloaded.ToString() + " images downloaded, " + results.TotalImagesSkipped.ToString() + " images skipped.");

            return results;
        }

        public static void ListDuplicateNames(string dataOutputPath)
        {
            var itemFile = new WikiFileHelper<WikiItemData>(getFilePath(dataOutputPath, ITEMS_FILE_NAME), getCorrectionsFilePath(dataOutputPath, ITEMS_CORRECTIONS_FILE_NAME));

            if (itemFile.IsEmpty())
            {
                Console.WriteLine("The item file has not been built yet");
                return;
            }
            
            var itemList = itemFile.ReadFromFile();

            Console.WriteLine("Searching " + itemFile.FilePath + " for items with identical names (case-insensitive):");

            var duplicateItems = itemList
                .GroupBy(item => item.Name.ToLower())
                .Where(group => group.Count() > 1)
                .SelectMany(group => group)
                .OrderBy(item => item.Name)
                .ToArray();

            string lastName = null;
            int uniqueNames = 1;
            foreach (WikiItemData item in duplicateItems)
            {
                if (lastName != null && !item.Name.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    uniqueNames++;
                    Console.WriteLine("--------------------------------------------------------------------" + Environment.NewLine);
                }

                Console.WriteLine("[" + item.WikiPageId + "] " + item.Name + Environment.NewLine + String.Join(Environment.NewLine, item.Stats) + Environment.NewLine);

                lastName = item.Name;
            }

            Console.WriteLine(duplicateItems.Length.ToString() + " total items, " + uniqueNames.ToString() + " unique names");
        }


        // private

        private static string getFilePath(string dataOutputPath, string fileName)
        {
            return Path.GetFullPath(Path.Join(dataOutputPath, fileName));
        }

        private static string getCorrectionsFilePath(string dataOutputPath, string correctionsFileName)
        {
            return Path.GetFullPath(Path.Join(dataOutputPath, correctionsFileName));
        }

        private static IEnumerable<WikiQueryResult> queryWiki(string wikiCategory)
        {
            var results = new List<WikiQueryResult>();

            using (var wc = new WebClient())
            {
                int pageCount = 1;
                string lastPageGcmContinue = null;
                do
                {
                    string nextPageUrl = QUERY_URL + wikiCategory;
                    if (!String.IsNullOrWhiteSpace(lastPageGcmContinue))
                        nextPageUrl += "&gcmcontinue=" + lastPageGcmContinue;

                    JObject result = JObject.Parse(wc.DownloadString(nextPageUrl));

                    string pageFirstName = null;
                    string pageLastName = null;
                    int pageEntryCount = 0;
                    foreach (JProperty prop in result["query"]["pages"].Children())
                    {
                        string pageId = prop.Name.Trim();
                        string name = prop.Value["title"].ToString().Trim();
                        if (!name.StartsWith("Category:", StringComparison.InvariantCultureIgnoreCase))
                        {
                            results.Add(new WikiQueryResult()
                            {
                                WikiPageId = pageId.Trim(),
                                Name = name.Trim()
                            });

                            pageEntryCount++;

                            if (pageFirstName == null)
                                pageFirstName = name;
                            pageLastName = name;
                        }
                    }

                    Console.WriteLine("[" + pageCount.ToString() + "] \"" + pageFirstName + "\" to \"" + pageLastName + "\" (" + pageEntryCount + " object names)");

                    lastPageGcmContinue = result["query-continue"]?["categorymembers"]?["gcmcontinue"]?.ToString();
                    pageCount++;
                    Thread.Sleep(250); // give the wiki server a chance to breathe (and hopefully avoid getting throttled)
                }
                while (!String.IsNullOrWhiteSpace(lastPageGcmContinue));
            }

            return results;
        }

        private static IEnumerable<WikiQueryResult> getOnlyP99Objects(IEnumerable<WikiQueryResult> objects, IEnumerable<WikiData> nonP99Objects)
        {
            return objects.Where(wikiObj => nonP99Objects.Where(nonP99Obj => nonP99Obj.WikiPageId == wikiObj.WikiPageId).Count() == 0);
        }


        // helper classes

        public class ScrapeResults
        {
            public int TotalDownloaded { get; set; } = 0;
            public int TotalSkipped { get; set; } = 0;
            public int TotalImagesDownloaded { get; set; } = 0;
            public int TotalImagesSkipped { get; set; } = 0;

            public void Include(ScrapeResults results)
            {
                this.TotalDownloaded += results.TotalDownloaded;
                this.TotalSkipped += results.TotalSkipped;
                this.TotalImagesDownloaded += results.TotalImagesDownloaded;
                this.TotalImagesSkipped += results.TotalImagesSkipped;
            }
        }

        public class WikiQueryResult
        {
            public string WikiPageId { get; set; }
            public string Name { get; set; }
        }
    }
}