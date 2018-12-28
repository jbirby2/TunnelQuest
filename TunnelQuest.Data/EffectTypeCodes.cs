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

        public static readonly IEnumerable<string> All = new string[] { Combat, Worn, ClickAnySlot, ClickEquipped };
    }
}
