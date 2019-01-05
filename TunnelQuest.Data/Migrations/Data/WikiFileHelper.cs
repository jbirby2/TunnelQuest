using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TunnelQuest.Data.Migrations.Data
{
    // Helper class to wrap up the code for reading/writing the json files that contain the scraped
    // wiki data (and any manual corrections).

    public class WikiFileHelper<T> : WikiJsonHelper<T> where T : WikiData
    {
        public string FilePath { get; private set;}
        public string CorrectionsFilePath { get; private set; }

        public WikiFileHelper(string filePath, string correctionsFilePath)
        {
            this.FilePath = filePath;
            this.CorrectionsFilePath = correctionsFilePath;
        }
        
        // This function is called by the DatabaseBuilder project, because it is working with the json file
        // before it gets compiled into the DLL as an embedded resource
        public List<T> ReadFromFile()
        {
            if (File.Exists(this.FilePath))
                return ResolveFromJson(File.ReadAllText(this.FilePath), File.ReadAllText(this.CorrectionsFilePath));
            else
                return new List<T>();
        }

        // This function is called by the DatabaseBuilder project while it's in the process of building
        // the json file, before it gets compiled into the DLL as an embedded resource
        public void WriteToFile(IEnumerable<T> itemList)
        {
            var serializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            var serializer = JsonSerializer.Create(serializerSettings);

            var itemListJObject = new JObject();
            foreach (var wikiItem in itemList)
            {
                itemListJObject.Add(wikiItem.WikiPageId, JToken.FromObject(wikiItem, serializer));
            }

            var outputFileInfo = new FileInfo(this.FilePath);
            if (!outputFileInfo.Directory.Exists)
                Directory.CreateDirectory(outputFileInfo.Directory.FullName);

            File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(itemListJObject, Formatting.Indented, serializerSettings));
        }
  
        public bool IsEmpty()
        {
            return (File.Exists(this.FilePath) == false || new FileInfo(this.FilePath).Length < 10);
        }
    }
}
