using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TunnelQuest.Data
{
    public static class ServerCodes
    {
        public const string Blue = "BLUE";
        public const string Red = "RED";

        public static readonly IEnumerable<string> All = new string[] { Blue, Red };

    }
}
