using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TunnelQuest.Core
{
    public static class ClassCodes
    {
        public const string Enchanter = "ENC";
        public const string Magician = "MAG";
        public const string Necromancer = "NEC";
        public const string Wizard = "WIZ";
        public const string Cleric = "CLR";
        public const string Druid = "DRU";
        public const string Shaman = "SHM";
        public const string Bard = "BRD";
        public const string Monk = "MNK";
        public const string Ranger = "RNG";
        public const string Rogue = "ROG";
        public const string Paladin = "PAL";
        public const string ShadowKnight = "SHD";
        public const string Warrior = "WAR";

        public static readonly IEnumerable<string> All = new string[] { Enchanter, Magician, Necromancer, Wizard, Cleric, Druid, Shaman, Bard, Monk, Ranger, Rogue, Paladin, ShadowKnight, Warrior };


        public static string GetCode(string className)
        {
            for (int i = 0; i < ClassCodes.All.Count(); i++)
            {
                if (ClassNames.All.ElementAt(i).Equals(className, StringComparison.InvariantCultureIgnoreCase))
                    return ClassCodes.All.ElementAt(i);
            }

            throw new Exception("Invalid class name " + className);
        }
    }
}
