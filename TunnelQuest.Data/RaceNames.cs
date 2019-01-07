using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class RaceNames
    {
        public const string Barbarian = "Barbarian";
        public const string DarkElf = "DarkElf";
        public const string Dwarf = "Dwarf";
        public const string Erudite = "Erudite";
        public const string Gnome = "Gnome";
        public const string HalfElf = "Half-Elf";
        public const string Halfling = "Halfling";
        public const string HighElf = "High Elf";
        public const string Human = "Human";
        public const string Iksar = "Iksar";
        public const string Ogre = "Ogre";
        public const string Troll = "Troll";
        public const string WoodElf = "Wood Elf";

        public static readonly IEnumerable<string> All = new string[] { Barbarian, DarkElf, Dwarf, Erudite, Gnome, HalfElf, Halfling, HighElf, Human, Iksar, Ogre, Troll, WoodElf };
    }
}
