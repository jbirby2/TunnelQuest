using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TunnelQuest.DatabaseBuilder
{
    public static class WikiScraper
    {
        public static readonly string BUILD_ITEM_LIST_URL = "http://wiki.project1999.com/api.php?action=query&generator=categorymembers&gcmtitle=Category:Items&gcmlimit=max&format=json";
        public static readonly string OUTPUT_WRAPPER_FOLDER_NAME = "wiki.project1999.com_Data_Scrapes";
        public static readonly string ITEM_LIST_FILE_NAME = "_item_list.txt";

        public static void BuildItemList(string fileOutputPath)
        {
            string outputFilePath = Path.Join(Path.Join(fileOutputPath, OUTPUT_WRAPPER_FOLDER_NAME), ITEM_LIST_FILE_NAME);
            var outputFileInfo = new FileInfo(outputFilePath);

            if (!outputFileInfo.Directory.Exists)
                Directory.CreateDirectory(outputFileInfo.Directory.FullName);
            File.WriteAllText(outputFilePath, String.Empty);

            Console.WriteLine("Rebuilding " + outputFilePath + " from the wiki:");

            DateTime startTime = DateTime.Now;
            int totalItems = 0;

            using (var wc = new WebClient())
            {
                int pageCount = 1;
                string lastPageGcmContinue = null;
                do
                {
                    string nextPageUrl = BUILD_ITEM_LIST_URL;
                    if (!String.IsNullOrWhiteSpace(lastPageGcmContinue))
                        nextPageUrl += "&gcmcontinue=" + lastPageGcmContinue;

                    JObject result = JObject.Parse(wc.DownloadString(nextPageUrl));

                    var itemNames = new List<string>();
                    foreach (JToken itemPage in result["query"]["pages"].Children().Values())
                    {
                        string itemName = itemPage["title"].ToString();
                        if (!itemName.StartsWith("Category:", StringComparison.InvariantCultureIgnoreCase))
                        {
                            itemNames.Add(itemName);
                            totalItems++;
                        }
                    }
                    File.AppendAllLines(outputFilePath, itemNames.ToArray());

                    Console.WriteLine("   [" + pageCount.ToString() + "] \"" + itemNames[0] + "\" to \"" + itemNames[itemNames.Count - 1] + "\"");

                    lastPageGcmContinue = result["query-continue"]?["categorymembers"]?["gcmcontinue"]?.ToString();
                    pageCount++;
                    Thread.Sleep(250); // give the wiki server a chance to breathe (and hopefully avoid getting throttled)
                }
                while (!String.IsNullOrWhiteSpace(lastPageGcmContinue));

                Console.WriteLine("Finished building list of " + totalItems.ToString() + " items in " + (DateTime.Now - startTime).TotalSeconds.ToString() + " seconds.  Press any key to exit.");
                Console.ReadKey();
            }

        }
    }
}