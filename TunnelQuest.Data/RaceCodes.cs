using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class RaceCodes
    {
        public static readonly string Barbarian = "BAR";
        public static readonly string DarkElf = "DEF";
        public static readonly string Dwarf = "DWF";
        public static readonly string Erudite = "ERU";
        public static readonly string Gnome = "GNM";
        public static readonly string HalfElf = "HEF";
        public static readonly string Halfling = "HFL";
        public static readonly string HighElf = "HIE";
        public static readonly string Human = "HUM";
        public static readonly string Iksar = "IKS";
        public static readonly string Ogre = "OGR";
        public static readonly string Troll = "TRL";
        public static readonly string WoodElf = "ELF";

        public static readonly IEnumerable<string> All = new string[] { Barbarian, DarkElf, Dwarf, Erudite, Gnome, HalfElf, Halfling, HighElf, Human, Iksar, Ogre, Troll, WoodElf };
    }
}
