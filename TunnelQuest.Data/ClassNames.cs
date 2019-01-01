using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TunnelQuest.Data
{
    public static class ClassNames
    {
        public static readonly string Enchanter = "Enchanter";
        public static readonly string Magician = "Magician";
        public static readonly string Necromancer = "Necromancer";
        public static readonly string Wizard = "Wizard";
        public static readonly string Cleric = "Cleric";
        public static readonly string Druid = "Druid";
        public static readonly string Shaman = "Shaman";
        public static readonly string Bard = "Bard";
        public static readonly string Monk = "Monk";
        public static readonly string Ranger = "Ranger";
        public static readonly string Rogue = "Rogue";
        public static readonly string Paladin = "Paladin";
        public static readonly string ShadowKnight = "Shadow Knight";
        public static readonly string Warrior = "Warrior";

        public static readonly IEnumerable<string> All = new string[] { Enchanter, Magician, Necromancer, Wizard, Cleric, Druid, Shaman, Bard, Monk, Ranger, Rogue, Paladin, ShadowKnight, Warrior };


        public static string GetName(string classCode)
        {
            for (int i = 0; i < ClassNames.All.Count(); i++)
            {
                if (ClassCodes.All.ElementAt(i).Equals(classCode, StringComparison.InvariantCultureIgnoreCase))
                    return ClassNames.All.ElementAt(i);
            }

            throw new Exception("Invalid class code " + classCode);
        }
    }
}
