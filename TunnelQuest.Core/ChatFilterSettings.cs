using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TunnelQuest.Core
{
    public class ChatFilterSettings
    {
        public bool? IsPermanent { get; set; } = null;
        public string[] ItemNames { get; set; } = null;
        public string PlayerName { get; set; } = null;
        public bool? IsBuying { get; set; } = null;
        public int? MinPrice { get; set; } = null;
        public int? MaxPrice { get; set; } = null;
        public int? MinGoodPriceDeviation { get; set; } = null;
        public int? MaxBadPriceDeviation { get; set; } = null;
    }
}
