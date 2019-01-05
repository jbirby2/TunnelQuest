using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class BuySellTradeSegment : BaseSegment
    {
        // static

        public static BuySellTradeSegment TryParse(ParsedChatLine parentLine, BaseSegment textSegment)
        {
            var upperText = textSegment.Text.ToUpper();

            // the "< 7" condition is just a sanity check to reduce the likelihood of trying to parse some weird player word that starts with "wt"
            if (upperText.Length > 2 && upperText.Length < 7 && upperText[0] == 'W' && upperText[1] == 'T')
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

                return new BuySellTradeSegment(parentLine, textSegment.Text, isBuying, isAcceptingTrades);
            }
            else
                return null;
        }


        // non-static

        public bool? IsBuying { get; private set; } = false;
        public bool? IsAcceptingTrades { get; private set; } = false;


        // protected constructor so that these segments can only be created by calling TryParse()
        protected BuySellTradeSegment(ParsedChatLine parentLine, string text, bool? isBuying, bool? isAcceptingTrades)
            : base(parentLine, text)
        {
            this.IsBuying = isBuying;
            this.IsAcceptingTrades = isAcceptingTrades;
        }
    }
}
