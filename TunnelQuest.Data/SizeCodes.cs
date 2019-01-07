using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class SizeCodes
    {
        public const string Tiny = "TINY";
        public const string Small = "SMALL";
        public const string Medium = "MEDIUM";
        public const string Large = "LARGE";
        public const string Giant = "GIANT";

        public static readonly IEnumerable<string> All = new string[] { Tiny, Small, Medium, Large, Giant };
    }
}
