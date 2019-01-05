using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class OrBestOfferSegment : BaseSegment
    {
        // static

        public static OrBestOfferSegment TryParse(ParsedChatLine parentLine, BaseSegment textSegment)
        {
            if (textSegment.Text.Equals("obo", StringComparison.InvariantCultureIgnoreCase))
                return new OrBestOfferSegment(parentLine, textSegment.Text);
            else if (textSegment.Text.Equals("or", StringComparison.InvariantCultureIgnoreCase))
            {
                var nextSegment = parentLine.NextSegment(textSegment);
                if (nextSegment != null && nextSegment.GetType() == typeof(BaseSegment) && nextSegment.Text.Equals("best", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nextNextSegment = parentLine.NextSegment(nextSegment);
                    if (nextNextSegment != null && nextNextSegment.GetType() == typeof(BaseSegment) && nextNextSegment.Text.Equals("offer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        parentLine.Segments.Remove(nextNextSegment);
                        parentLine.Segments.Remove(nextSegment);
                        return new OrBestOfferSegment(parentLine, textSegment.Text + ' ' + nextSegment.Text + ' ' + nextNextSegment.Text);
                    }
                }
            }

            return null;
        }


        // non-static


        // protected constructor so that these segments can only be created by calling TryParse()
        protected OrBestOfferSegment(ParsedChatLine parentLine, string text)
            : base(parentLine, text)
        {
        }
    }
}
