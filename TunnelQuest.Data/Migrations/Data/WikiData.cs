using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data.Migrations.Data
{
    public class WikiData
    {
        [JsonIgnore]
        public string WikiPageId { get; set; }


        public string Name { get; set; }

        public override string ToString()
        {
            return base.ToString() + " " + Name;
        }
    }
}
