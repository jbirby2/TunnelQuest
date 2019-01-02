using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TunnelQuest.Data
{
    public static class ServerCodes
    {
        public static readonly string Blue = "BLUE";
        public static readonly string Red = "RED";

        public static readonly IEnumerable<string> All = new string[] { Blue, Red };

    }
}
