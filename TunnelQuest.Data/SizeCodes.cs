using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class SizeCodes
    {
        public static readonly string Tiny = "TINY";
        public static readonly string Small = "SMALL";
        public static readonly string Medium = "MEDIUM";
        public static readonly string Large = "LARGE";
        public static readonly string Giant = "GIANT";

        public static readonly IEnumerable<string> All = new string[] { Tiny, Small, Medium, Large, Giant };
    }
}
