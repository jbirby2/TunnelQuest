using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class PriceSegment : TextSegment
    {
        // static

        public static PriceSegment TryParse(ParsedChatLine parentLine, TextSegment textSegment)
        {
            // assume a price starts with numbers, then ends with non-numeric characters (e.g. "200p", "10k", "10k!!!!!!!!~~~", etc)
            string numberString = "";
            bool foundDecimalPoint = false;
            foreach (char chr in textSegment.Text)
            {
                if (Char.IsNumber(chr))
                    numberString += chr;
                else if (foundDecimalPoint == false && chr == '.')
                {
                    foundDecimalPoint = true;
                    numberString += chr;
                }
                else
                    break;
            }

            if (numberString != "")
            {
                // Remember that numberString could contain a decimal point, e.g. "3.5k"
                double price = Double.Parse(numberString);

                string charsAfterNumber = textSegment.Text.Substring(numberString.Length).ToLowerInvariant();
                if (charsAfterNumber.Length > 0)
                {
                    if (charsAfterNumber[0] == 'x' || charsAfterNumber[0] == 'X')
                        return null;
                    else if (charsAfterNumber[0] == 'k' || charsAfterNumber[0] == 'K')
                        price = price * 1000;
                }
                else
                {
                    // This is a pure number with no characters at the end; make sure that neither the previous
                    // nor next segments are the text "x", such as in "WTS 12 x Goblin Ear" or "WTS Goblin Ear x 12"
                    var nextSegment = textSegment.NextSegment();
                    if (nextSegment != null && nextSegment.GetType() == typeof(TextSegment) && nextSegment.Text.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                        return null;
                    var prevSegment = textSegment.PrevSegment();
                    if (prevSegment != null && prevSegment.GetType() == typeof(TextSegment) && prevSegment.Text.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                        return null;
                }

                // don't allow any nonsense prices
                if (price <= 0)
                    return null;
                else
                    return new PriceSegment(parentLine, textSegment.Text, Convert.ToInt32(price), textSegment.HasPrecedingSpace);
            }
            else
                return null;
        }

        // non-static

        public int Price { get; private set; }

        // These are set during the parsing process and used only to generate the Text.  Kinda ugly and janky but it's only touched in one spot.
        public bool IsBuying { get; set; }
        public List<int> UsedByItemIndexes { get; set; }

        // override the default Text behavior
        public override string Text
        {
            get
            {
                if (this.UsedByItemIndexes.Count > 0)
                    return ChatLogic.OUTER_CHAT_TOKEN + "price" + ChatLogic.INNER_CHAT_TOKEN + (this.IsBuying ? '1' : '0') + ChatLogic.INNER_CHAT_TOKEN + this.Price.ToString() + ChatLogic.INNER_CHAT_TOKEN + String.Join(',', this.UsedByItemIndexes) + ChatLogic.INNER_CHAT_TOKEN + base.Text + ChatLogic.OUTER_CHAT_TOKEN;
                else
                    return base.Text;
            }
        }

        // protected constructor so that these segments can only be created by calling TryParse()
        protected PriceSegment(ParsedChatLine parentLine, string text, int price, bool hasPrecedingSpace)
            : base(parentLine, text, hasPrecedingSpace)
        {
            this.Price = price;
            this.IsBuying = false;
            this.UsedByItemIndexes = new List<int>();
        }
    }
}
