using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class SlotCodes
    {
        public static readonly string Arms = "ARMS";
        public static readonly string Back = "BACK";
        public static readonly string Chest = "CHEST";
        public static readonly string Ear = "EAR";
        public static readonly string Face = "FACE";
        public static readonly string Feet = "FEET";
        public static readonly string Finger = "FINGER";
        public static readonly string Hands = "HANDS";
        public static readonly string Head = "HEAD";
        public static readonly string Legs = "LEGS";
        public static readonly string Neck = "NECK";
        public static readonly string Shoulders = "SHOULDERS";
        public static readonly string Waist = "WAIST";
        public static readonly string Wrist = "WRIST";
        public static readonly string Primary = "PRIMARY";
        public static readonly string Secondary = "SECONDARY";
        public static readonly string Range = "RANGE";
        public static readonly string Ammo = "AMMO";

        public static readonly IEnumerable<string> All = new string[] { Arms, Back, Chest, Ear, Face, Feet, Finger, Hands, Head, Legs, Neck, Shoulders, Waist, Wrist, Primary, Secondary, Range, Ammo };
    }
}
