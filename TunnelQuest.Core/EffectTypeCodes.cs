using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Core
{
    public static class EffectTypeCodes
    {
        public const string Combat = "Combat";
        public const string Worn = "Worn";
        public const string ClickAnySlot = "ClickAnySlot";
        public const string ClickEquipped = "ClickEquipped";
        public const string LearnSpell = "LearnSpell"; // used only for spell scroll items, e.g. "Spell: Minor Healing" item

        public static readonly IEnumerable<string> All = new string[] { Combat, Worn, ClickAnySlot, ClickEquipped, LearnSpell };
    }
}
