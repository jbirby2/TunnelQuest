using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TunnelQuest.Data
{
    public static class ClassNames
    {
        public const string Enchanter = "Enchanter";
        public const string Magician = "Magician";
        public const string Necromancer = "Necromancer";
        public const string Wizard = "Wizard";
        public const string Cleric = "Cleric";
        public const string Druid = "Druid";
        public const string Shaman = "Shaman";
        public const string Bard = "Bard";
        public const string Monk = "Monk";
        public const string Ranger = "Ranger";
        public const string Rogue = "Rogue";
        public const string Paladin = "Paladin";
        public const string ShadowKnight = "Shadow Knight";
        public const string Warrior = "Warrior";

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
