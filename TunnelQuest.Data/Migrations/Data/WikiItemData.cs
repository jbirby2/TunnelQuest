﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TunnelQuest.Data.Migrations.Data
{
    public class WikiItemData
    {
        // static helpers

        // This function is called by the migration code, so that it doesn't have to rely on the json file
        // being physically present at runtime
        public static List<WikiItemData> ReadFromEmbeddedResource()
        {
            string itemsJson;
            string correctionsJson;

            var assembly = typeof(WikiItemData).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("TunnelQuest.Data.Migrations.Data.wiki.project1999.com_items_scrape.json"))
            using (var reader = new StreamReader(stream))
            {
                itemsJson = reader.ReadToEnd();
            }

            using (var stream = assembly.GetManifestResourceStream("TunnelQuest.Data.Migrations.Data.wiki_scrape_corrections.json"))
            using (var reader = new StreamReader(stream))
            {
                correctionsJson = reader.ReadToEnd();
            }

            return ReadFromJson(itemsJson, correctionsJson);
        }

        // This function is called by the DatabaseBuilder project, because it is working with the json file
        // before it gets compiled into the DLL as an embedded resource
        public static List<WikiItemData> ReadFromFile(string itemsFilePath, string correctionsFilePath)
        {
            if (File.Exists(itemsFilePath))
                return ReadFromJson(File.ReadAllText(itemsFilePath), File.ReadAllText(correctionsFilePath));
            else
                return new List<WikiItemData>();
        }

        public static List<WikiItemData> ReadFromJson(string itemsJsonString, string correctionsJsonString)
        {
            var items = readFromJson(itemsJsonString);
            var correctedItems = readFromJson(correctionsJsonString);

            items.RemoveAll(item => correctedItems.Find(correctItem => correctItem.WikiPageId.Equals(item.WikiPageId, StringComparison.InvariantCultureIgnoreCase)) != null);
            items.AddRange(correctedItems);

            return items;
        }
        
        // This function is called by the DatabaseBuilder project while it's in the process of building
        // the json file, before it gets compiled into the DLL as an embedded resource
        public static void WriteToFile(string itemListFilePath, IEnumerable<WikiItemData> itemList)
        {
            var serializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            var serializer = JsonSerializer.Create(serializerSettings);

            var itemListJObject = new JObject();
            foreach (var wikiItem in itemList)
            {
                itemListJObject.Add(wikiItem.WikiPageId, JToken.FromObject(wikiItem, serializer));
            }

            var outputFileInfo = new FileInfo(itemListFilePath);
            if (!outputFileInfo.Directory.Exists)
                Directory.CreateDirectory(outputFileInfo.Directory.FullName);

            File.WriteAllText(itemListFilePath, JsonConvert.SerializeObject(itemListJObject, Formatting.Indented, serializerSettings));
        }


        private static List<WikiItemData> readFromJson(string itemsJsonString)
        {
            var itemList = new List<WikiItemData>();

            if (!String.IsNullOrWhiteSpace(itemsJsonString))
            {
                var itemListJObject = JObject.Parse(itemsJsonString);

                foreach (JProperty nextItemProp in itemListJObject.Properties())
                {
                    WikiItemData nextItem = nextItemProp.Value.ToObject<WikiItemData>();
                    nextItem.WikiPageId = nextItemProp.Name;
                    itemList.Add(nextItem);
                }
            }

            return itemList;
        }


        // non-static

        [JsonIgnore]
        public string WikiPageId { get; set; }

        public string ItemName { get; set; }
        public string IconFileName { get; set; }
        public string[] Stats { get; set; }

        public override string ToString()
        {
            return "[" + WikiPageId + "] " + ItemName;
        }
    }
}
