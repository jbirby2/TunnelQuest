using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class NumberSegment : ChatSegment
    {
        // static

        public static NumberSegment TryParse(string text)
        {
            // assume a price starts with numbers, then ends with non-numeric characters (e.g. "200p", "10k", "10k!!!!!!!!~~~", etc)
            string numberString = "";
            foreach (char chr in text)
            {
                if (Char.IsNumber(chr))
                    numberString += chr;
                else
                    break;
            }

            if (numberString != "")
            {
                int price = Int32.Parse(numberString);

                string charsAfterNumber = numberString.Substring(numberString.Length).ToLowerInvariant();
                if (charsAfterNumber.Length > 0)
                {
                    if (charsAfterNumber[0] == 'k' || charsAfterNumber[0] == 'K')
                        price = price * 10;
                }

                return new NumberSegment(text, price);
            }
            else
                return null;
        }

        // non-static

        public int Price { get; private set; }

        // protected constructor so that these segments can only be created by calling TryParse()
        protected NumberSegment(string text, int price)
            : base(text)
        {
        }
    }
}
