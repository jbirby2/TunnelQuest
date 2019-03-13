using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TunnelQuest.Core.Migrations.Data
{
    // Helper class to wrap up the code for reading json files that are embedded into the DLL
    // as Embedded Resources.  This is how the migration gets the data from the json files, so that
    // the actual .json files themselves don't have to be published alongside the compiled executable files.

    public class WikiResourceHelper<T> : WikiJsonHelper<T> where T : WikiData
    {
        public string ResourceName { get; private set; }
        public string CorrectionsResourceName { get; private set; }

        public WikiResourceHelper(string resourceName, string correctionsResourceName)
        {
            this.ResourceName = resourceName;
            this.CorrectionsResourceName = correctionsResourceName;
        }

        public List<T> ReadFromEmbeddedResource()
        {
            string json;
            string correctionsJson;

            var assembly = typeof(T).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("TunnelQuest.Core.Migrations.Data.WikiScrapes." + this.ResourceName))
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            using (var stream = assembly.GetManifestResourceStream("TunnelQuest.Core.Migrations.Data.WikiScrapes." + this.CorrectionsResourceName))
            using (var reader = new StreamReader(stream))
            {
                correctionsJson = reader.ReadToEnd();
            }

            return ResolveFromJson(json, correctionsJson);
        }
        
    }
}
