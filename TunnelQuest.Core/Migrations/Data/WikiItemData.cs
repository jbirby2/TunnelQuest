using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TunnelQuest.Core.Migrations.Data
{
    public class WikiItemData : WikiData
    {
        public string IconFileName { get; set; }
        public string[] Stats { get; set; } = new string[0];
    }
}
