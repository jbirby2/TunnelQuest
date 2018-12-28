using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class RaceNames
    {
        public static readonly string Barbarian = "Barbarian";
        public static readonly string DarkElf = "DarkElf";
        public static readonly string Dwarf = "Dwarf";
        public static readonly string Erudite = "Erudite";
        public static readonly string Gnome = "Gnome";
        public static readonly string HalfElf = "Half-Elf";
        public static readonly string Halfling = "Halfling";
        public static readonly string HighElf = "High Elf";
        public static readonly string Human = "Human";
        public static readonly string Iksar = "Iksar";
        public static readonly string Ogre = "Ogre";
        public static readonly string Troll = "Troll";
        public static readonly string WoodElf = "Wood Elf";

        public static readonly IEnumerable<string> All = new string[] { Barbarian, DarkElf, Dwarf, Erudite, Gnome, HalfElf, Halfling, HighElf, Human, Iksar, Ogre, Troll, WoodElf };
    }
}
