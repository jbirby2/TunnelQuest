using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class StatCodes
    {
        public static readonly string Strength = "STR";
        public static readonly string Stamina = "STA";
        public static readonly string Agility = "AGI";
        public static readonly string Dexterity = "DEX";
        public static readonly string Wisdom = "WIS";
        public static readonly string Intelligence = "INT";
        public static readonly string Charisma = "CHA";
        public static readonly string HitPoints = "HP";
        public static readonly string Mana = "MANA";
        public static readonly string ArmorClass = "AC";
        public static readonly string MagicResist = "SV MAGIC";
        public static readonly string PoisonResist = "SV POISON";
        public static readonly string DiseaseResist = "SV DISEASE";
        public static readonly string FireResist = "SV FIRE";
        public static readonly string ColdResist = "SV COLD";

        public static readonly IEnumerable<string> All = new string[] { Strength, Stamina, Agility, Dexterity, Wisdom, Intelligence, Charisma, HitPoints, Mana, ArmorClass, MagicResist, PoisonResist, DiseaseResist, FireResist, ColdResist };
    }
}
