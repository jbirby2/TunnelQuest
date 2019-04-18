using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Core.ChatSegments
{
    internal class BuySellTradeSegment : TextSegment
    {
        // static

        public static BuySellTradeSegment TryParse(TextSegment textSegment)
        {
            var upperText = textSegment.Text.ToUpper();

            if (upperText == "BUYING")
                return new BuySellTradeSegment(textSegment.Text, true, null, textSegment.HasPrecedingSpace);

            // the "< 7" condition is just a sanity check to reduce the likelihood of trying to parse some weird player word that starts with "wt"
            else if (upperText.Length > 2 && upperText.Length < 7 && upperText[0] == 'W' && upperText[1] == 'T')
            {
                // sigh.
                if (upperText.StartsWith("WTF"))
                    return null;

                bool? isBuying = null;
                bool? isAcceptingTrades = null;

                // check every character after the "WT" in the beginning
                for (int i = 2; i < upperText.Length; i++)
                {
                    if (upperText[i] == 'B')
                        isBuying = true;
                    else if (upperText[i] == 'T')
                        isAcceptingTrades = true;
                }

                return new BuySellTradeSegment(textSegment.Text, isBuying, isAcceptingTrades, textSegment.HasPrecedingSpace);
            }
            else
                return null;
        }


        // non-static

        public bool? IsBuying { get; private set; } = false;
        public bool? IsAcceptingTrades { get; private set; } = false;


        // protected constructor so that these segments can only be created by calling TryParse()
        protected BuySellTradeSegment(string text, bool? isBuying, bool? isAcceptingTrades, bool hasPrecedingSpace)
            : base(text, hasPrecedingSpace)
        {
            this.IsBuying = isBuying;
            this.IsAcceptingTrades = isAcceptingTrades;
        }
    }
}
