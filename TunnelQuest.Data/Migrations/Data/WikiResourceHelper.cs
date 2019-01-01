using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TunnelQuest.Data.Migrations.Data
{
    public class WikiResourceHelper<T> : WikiJsonHelper<T> where T : WikiData
    {
        public string ResourceName { get; private set; }
        public string CorrectionsResourceName { get; private set; }

        public WikiResourceHelper(string resourceName, string correctionsResourceName)
        {
            this.ResourceName = resourceName;
            this.CorrectionsResourceName = correctionsResourceName;
        }

        // This function is called by the migration code, so that it doesn't have to rely on the json file
        // being physically present at runtime
        public List<T> ReadFromEmbeddedResource()
        {
            string json;
            string correctionsJson;

            var assembly = typeof(T).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("TunnelQuest.Data.Migrations.Data.WikiScrapes." + this.ResourceName))
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            using (var stream = assembly.GetManifestResourceStream("TunnelQuest.Data.Migrations.Data.WikiScrapes." + this.CorrectionsResourceName))
            using (var reader = new StreamReader(stream))
            {
                correctionsJson = reader.ReadToEnd();
            }

            return ResolveFromJson(json, correctionsJson);
        }
        
    }
}
