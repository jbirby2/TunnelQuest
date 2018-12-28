using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class ClassCodes
    {
        public static readonly string Enchanter = "ENC";
        public static readonly string Magician = "MAG";
        public static readonly string Necromancer = "NEC";
        public static readonly string Wizard = "WIZ";
        public static readonly string Cleric = "CLR";
        public static readonly string Druid = "DRU";
        public static readonly string Shaman = "SHM";
        public static readonly string Bard = "BRD";
        public static readonly string Monk = "MNK";
        public static readonly string Ranger = "RNG";
        public static readonly string Rogue = "ROG";
        public static readonly string Paladin = "PAL";
        public static readonly string ShadowKnight = "SHD";
        public static readonly string Warrior = "WAR";

        public static readonly IEnumerable<string> All = new string[] { Enchanter, Magician, Necromancer, Wizard, Cleric, Druid, Shaman, Bard, Monk, Ranger, Rogue, Paladin, ShadowKnight, Warrior };
    }
}
