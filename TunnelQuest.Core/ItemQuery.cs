using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Core
{
    public class ItemQuery
    {
        public string QueryType { get; set; } = "name";
        public IEnumerable<string> Names { get; set; } = new string[0];
        public ItemQueryStats Stats = new ItemQueryStats();
    }

    public class ItemQueryStats
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
        public int? MinPoisonResist { get; set; } = null;
        public int? MinDiseaseResist { get; set; } = null;
        public int? MinFireResist { get; set; } = null;
        public int? MinColdResist { get; set; } = null;
        public float? MinHaste { get; set; } = null;
        public int? MinSingingModifier { get; set; } = null;
        public int? MinPercussionModifier { get; set; } = null;
        public int? MinStringedModifier { get; set; } = null;
        public int? MinBrassModifier { get; set; } = null;
        public int? MinWindModifier { get; set; } = null;
    }
}
