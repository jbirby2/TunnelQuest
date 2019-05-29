using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TunnelQuest.Core
{
    public class ChatFilterSettings
    {
        public bool? IsPermanent { get; set; } = null;
        public string PlayerName { get; set; } = null;
        public bool? IsBuying { get; set; } = null;
        public ChatFilterSettingsItems Items = new ChatFilterSettingsItems();
        public int? MinPrice { get; set; } = null;
        public int? MaxPrice { get; set; } = null;
        public int? MinGoodPriceDeviation { get; set; } = null;
        public int? MaxBadPriceDeviation { get; set; } = null;
    }

    public class ChatFilterSettingsItems
    {
        public string FilterType { get; set; } = "name";
        public IEnumerable<string> Names { get; set; } = new string[0];
        public ChatFilterSettingsItemsStats Stats = new ChatFilterSettingsItemsStats();
    }

    public enum ChatFilterSettingsItemFilterType { Names = 1, Stats = 2}

    public class ChatFilterSettingsItemsStats
    {
        public int? MinStrength { get; set; } = null;
        public int? MinStamina { get; set; } = null;
        public int? MinAgility { get; set; } = null;
        public int? MinDexterity { get; set; } = null;
        public int? MinWisdom { get; set; } = null;
        public int? MinIntelligence { get; set; } = null;
        public int? MinCharisma { get; set; } = null;
        public int? MinHitPoints { get; set; } = null;
        public int? MinMana { get; set; } = null;
        public int? MinArmorClass { get; set; } = null;
        public int? MinMagicResist { get; set; } = null;
    }
}
