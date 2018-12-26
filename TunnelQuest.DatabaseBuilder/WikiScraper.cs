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
using TunnelQuest.Data.Migrations.Data;

namespace TunnelQuest.DatabaseBuilder
{
    public static class WikiScraper
    {
        public static readonly string BUILD_ITEM_LIST_URL = "http://wiki.project1999.com/api.php?action=query&generator=categorymembers&gcmtitle=Category:Items&gcmlimit=max&format=json";
        public static readonly string GET_ITEM_DETAIL_URL = "http://wiki.project1999.com/";
        public static readonly string ITEM_LIST_FILE_NAME = "wiki.project1999.com_items_scrape.json";
        public static readonly string IMAGE_WRAPPER_FOLDER_NAME = "wiki.project1999.com_items_scrape";

        
        public static void BuildItemDetails(string dataOutputPath, string imageOutputPath, bool pauseOnError)
        {
            string itemListFilePath = getItemListFilePath(dataOutputPath);

            if (!File.Exists(itemListFilePath))
                buildItemList(itemListFilePath);

            var itemList = WikiItemData.ReadFromFile(itemListFilePath);

            DateTime startTime = DateTime.Now;
            int totalFilesBuilt = 0;
            int totalFilesSkipped = 0;
            int totalImagesDownloaded = 0;
            int totalImagesSkipped = 0;

            using (var webClient = new WebClient())
            {
                Console.WriteLine("Searching " + itemListFilePath + " for missing item data or images to pull from the wiki:");

                foreach (WikiItemData nextItem in itemList)
                {
                    try
                    {
                        // download the wiki entry for the item, if necessary

                        if (String.IsNullOrWhiteSpace(nextItem.WikiPageId) == false && (String.IsNullOrWhiteSpace(nextItem.IconFileName) || nextItem.Stats == null || nextItem.Stats.Length == 0))
                        {
                            string itemHtml = webClient.DownloadString(Path.Join(GET_ITEM_DETAIL_URL, nextItem.ItemName));

                            var itemWikiDoc = new HtmlDocument();
                            itemWikiDoc.LoadHtml(itemHtml);

                            var dataElement = itemWikiDoc.DocumentNode.Descendants("div").Where(d => d.Attributes["class"]?.Value == "itemdata").FirstOrDefault();
                            if (dataElement == null)
                                throw new Exception("Couldn't find dataElement");

                            // set itemData.Stats

                            var statsElement = dataElement.Descendants("p").FirstOrDefault();
                            if (statsElement == null)
                                throw new Exception("Couldn't find statsElement");
                            nextItem.Stats = statsElement.InnerText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            // set itemData.IconFileName

                            var imageElement = dataElement.Descendants("img").FirstOrDefault();
                            if (imageElement == null)
                                throw new Exception("Couldn't find imageElement");
                            string imageSrc = imageElement.Attributes["src"].Value;
                            if (imageSrc.StartsWith('/'))
                                imageSrc = imageSrc.Remove(0, 1);
                            nextItem.IconFileName = imageSrc.Split('/')[1];

                            // re-write the data file to disk after every single item
                            WikiItemData.WriteToFile(itemListFilePath, itemList);

                            Console.WriteLine("[" + nextItem.WikiPageId + "] " + nextItem.ItemName);
                            totalFilesBuilt++;
                            Thread.Sleep(250);
                        }
                        else
                            totalFilesSkipped++;


                        // download the image file, if necessary

                        string imageUrl = Path.Join(GET_ITEM_DETAIL_URL + "images/", nextItem.IconFileName);
                        string imageFilePath = Path.GetFullPath(Path.Join(Path.Join(imageOutputPath, IMAGE_WRAPPER_FOLDER_NAME), nextItem.IconFileName));

                        if (!File.Exists(imageFilePath))
                        {
                            var imageFileInfo = new FileInfo(imageFilePath);

                            if (!imageFileInfo.Directory.Exists)
                                Directory.CreateDirectory(imageFileInfo.Directory.FullName);

                            webClient.DownloadFile(imageUrl, imageFilePath);
                            totalImagesDownloaded++;
                        }
                        else
                            totalImagesSkipped++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error pulling " + nextItem.ItemName);
                        Console.WriteLine(ex.ToString());
                        if (pauseOnError)
                        {
                            Console.WriteLine("Press any key to continue building items");
                            Console.ReadKey();
                        }
                    } // end catch
                } // end foreach (itemListLine)
            }

            Console.WriteLine("Finished in " + (DateTime.Now - startTime).TotalMinutes.ToString() + " minutes.  " + totalFilesBuilt.ToString() + " files built, " + totalFilesSkipped.ToString() + " files skipped, " + totalImagesDownloaded.ToString() + " images downloaded, " + totalImagesSkipped.ToString() + " images skipped.");
        }

        public static void ListDuplicateNames(string dataOutputPath)
        {
            string itemListFilePath = getItemListFilePath(dataOutputPath);

            if (!File.Exists(itemListFilePath))
            {
                Console.WriteLine("No file found at " + itemListFilePath);
                return;
            }

            var itemList = WikiItemData.ReadFromFile(itemListFilePath);

            Console.WriteLine("Searching " + itemListFilePath + " for items with identical names (case-insensitive):");

            var duplicateItems = itemList
                .GroupBy(item => item.ItemName.ToLower())
                .Where(group => group.Count() > 1)
                .SelectMany(group => group)
                .OrderBy(item => item.ItemName)
                .ToArray();

            foreach (WikiItemData item in duplicateItems)
            {
                Console.WriteLine("[" + item.WikiPageId + "] " + item.ItemName);
            }
        }


        // private

        private static void buildItemList(string itemListFilePath)
        {
            Console.WriteLine("Rebuilding " + itemListFilePath + " from the wiki:");

            DateTime startTime = DateTime.Now;
            int totalItems = 0;

            using (var wc = new WebClient())
            {
                var itemList = new List<WikiItemData>();
                int pageCount = 1;
                string lastPageGcmContinue = null;
                do
                {
                    string nextPageUrl = BUILD_ITEM_LIST_URL;
                    if (!String.IsNullOrWhiteSpace(lastPageGcmContinue))
                        nextPageUrl += "&gcmcontinue=" + lastPageGcmContinue;

                    JObject result = JObject.Parse(wc.DownloadString(nextPageUrl));

                    string pageFirstItemName = null;
                    string pageLastItemName = null;
                    int pageItemCount = 0;
                    foreach (JProperty prop in result["query"]["pages"].Children())
                    {
                        string pageId = prop.Name;
                        string itemName = prop.Value["title"].ToString();
                        if (!itemName.StartsWith("Category:", StringComparison.InvariantCultureIgnoreCase))
                        {
                            itemList.Add(new WikiItemData()
                            {
                                WikiPageId = pageId,
                                ItemName = itemName
                            });

                            pageItemCount++;
                            totalItems++;

                            if (pageFirstItemName == null)
                                pageFirstItemName = itemName;
                            pageLastItemName = itemName;
                        }
                    }

                    // re-write the data file to disk after every page of item names
                    WikiItemData.WriteToFile(itemListFilePath, itemList);

                    Console.WriteLine("[" + pageCount.ToString() + "] \"" + pageFirstItemName + "\" to \"" + pageLastItemName + "\" (" + pageItemCount + " items)");

                    lastPageGcmContinue = result["query-continue"]?["categorymembers"]?["gcmcontinue"]?.ToString();
                    pageCount++;
                    Thread.Sleep(250); // give the wiki server a chance to breathe (and hopefully avoid getting throttled)
                }
                while (!String.IsNullOrWhiteSpace(lastPageGcmContinue));

                Console.WriteLine("Finished building list of " + totalItems.ToString() + " items in " + (DateTime.Now - startTime).TotalSeconds.ToString() + " seconds.");
            }
        }

        private static string getItemListFilePath(string dataOutputPath)
        {
            return Path.GetFullPath(Path.Join(dataOutputPath, ITEM_LIST_FILE_NAME));
        }
        
    }
}