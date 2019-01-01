using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class EffectTypeCodes
    {
        public static readonly string Combat = "Combat";
        public static readonly string Worn = "Worn";
        public static readonly string ClickAnySlot = "ClickAnySlot";
        public static readonly string ClickEquipped = "ClickEquipped";
        public static readonly string LearnSpell = "LearnSpell"; // used only for spell scroll items, e.g. "Spell: Minor Healing" item

        public static readonly IEnumerable<string> All = new string[] { Combat, Worn, ClickAnySlot, ClickEquipped, LearnSpell };
    }
}
