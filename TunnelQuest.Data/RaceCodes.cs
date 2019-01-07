using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class RaceCodes
    {
        public const string Barbarian = "BAR";
        public const string DarkElf = "DEF";
        public const string Dwarf = "DWF";
        public const string Erudite = "ERU";
        public const string Gnome = "GNM";
        public const string HalfElf = "HEF";
        public const string Halfling = "HFL";
        public const string HighElf = "HIE";
        public const string Human = "HUM";
        public const string Iksar = "IKS";
        public const string Ogre = "OGR";
        public const string Troll = "TRL";
        public const string WoodElf = "ELF";

        public static readonly IEnumerable<string> All = new string[] { Barbarian, DarkElf, Dwarf, Erudite, Gnome, HalfElf, Halfling, HighElf, Human, Iksar, Ogre, Troll, WoodElf };
    }
}
