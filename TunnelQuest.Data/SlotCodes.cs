using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class SlotCodes
    {
        public const string Arms = "ARMS";
        public const string Back = "BACK";
        public const string Chest = "CHEST";
        public const string Ear = "EAR";
        public const string Face = "FACE";
        public const string Feet = "FEET";
        public const string Finger = "FINGER";
        public const string Hands = "HANDS";
        public const string Head = "HEAD";
        public const string Legs = "LEGS";
        public const string Neck = "NECK";
        public const string Shoulders = "SHOULDERS";
        public const string Waist = "WAIST";
        public const string Wrist = "WRIST";
        public const string Primary = "PRIMARY";
        public const string Secondary = "SECONDARY";
        public const string Range = "RANGE";
        public const string Ammo = "AMMO";

        public static readonly IEnumerable<string> All = new string[] { Arms, Back, Chest, Ear, Face, Feet, Finger, Hands, Head, Legs, Neck, Shoulders, Waist, Wrist, Primary, Secondary, Range, Ammo };
    }
}
