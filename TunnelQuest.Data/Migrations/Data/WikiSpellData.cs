using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TunnelQuest.Data.Migrations.Data
{
    public class WikiSpellData : WikiData
    {
        public string IconFileName { get; set; }
        public string Description { get; set; }
        public WikiSpellDataRequirements[] Requirements { get; set; } = new WikiSpellDataRequirements[0];
        public string[] EffectDetails { get; set; } = new string[0];
        public string[] Sources { get; set; } = new string[0];

        // helper classes

        public class WikiSpellDataRequirements
        {
            public string ClassCode { get; set; }
            public int Level { get; set; }
        }
    }
}
