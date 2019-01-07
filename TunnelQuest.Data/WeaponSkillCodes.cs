using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class WeaponSkillCodes
    {
        public const string OneHandBlunt = "1H Blunt";
        public const string TwoHandBlunt = "2H Blunt";
        public const string OneHandSlashing = "1H Slashing";
        public const string TwoHandSlashing = "2H Slashing";
        public const string OneHandPiercing = "Piercing";
        public const string TwoHandPiercing = "2H Piercing";
        public const string Archery = "Archery";
        public const string Throwing = "Throwing";
        public const string HandToHand = "Hand to Hand";

        public static readonly IEnumerable<string> All = new string[] { OneHandBlunt, TwoHandBlunt, OneHandSlashing, TwoHandSlashing, OneHandPiercing, TwoHandPiercing, Archery, Throwing, HandToHand };
    }
}
