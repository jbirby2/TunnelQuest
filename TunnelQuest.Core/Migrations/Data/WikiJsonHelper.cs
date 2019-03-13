using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace TunnelQuest.Core.Migrations.Data
{
    public class WikiJsonHelper<T> where T : WikiData
    {
        protected List<T> ResolveFromJson(string jsonString, string correctionsJsonString)
        {
            var objects = readFromJson(jsonString);
            var correctedObjects = readFromJson(correctionsJsonString);

            for (int i = 0; i < objects.Count; i++)
            {
                var correctedObject = correctedObjects.Find(correctObj => correctObj.WikiPageId.Equals(objects[i].WikiPageId, StringComparison.InvariantCultureIgnoreCase));
                if (correctedObject != null)
                    objects[i] = correctedObject;
            }

            return objects;
        }


        // private

        private List<T> readFromJson(string jsonString)
        {
            var objects = new List<T>();

            if (!String.IsNullOrWhiteSpace(jsonString))
            {
                var jObject = JObject.Parse(jsonString);

                foreach (JProperty nextProp in jObject.Properties())
                {
                    T nextObject = nextProp.Value.ToObject<T>();
                    nextObject.WikiPageId = nextProp.Name;
                    objects.Add(nextObject);
                }
            }

            return objects;
        }

    }
}
