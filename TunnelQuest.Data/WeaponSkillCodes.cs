using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class WeaponSkillCodes
    {
        public static readonly string OneHandBlunt = "1H Blunt";
        public static readonly string TwoHandBlunt = "2H Blunt";
        public static readonly string OneHandSlashing = "1H Slashing";
        public static readonly string TwoHandSlashing = "2H Slashing";
        public static readonly string OneHandPiercing = "Piercing";
        public static readonly string TwoHandPiercing = "2H Piercing";
        public static readonly string Archery = "Archery";
        public static readonly string Throwing = "Throwing";
        public static readonly string HandToHand = "Hand to Hand";

        public static readonly IEnumerable<string> All = new string[] { OneHandBlunt, TwoHandBlunt, OneHandSlashing, TwoHandSlashing, OneHandPiercing, TwoHandPiercing, Archery, Throwing, HandToHand };
    }
}
