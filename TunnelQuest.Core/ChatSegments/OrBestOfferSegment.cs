using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Core.ChatSegments
{
    internal class OrBestOfferSegment : TextSegment
    {
        // static

        public static OrBestOfferSegment TryParse(List<TextSegment> segments, int textSegmentIndex)
        {
            var textSegment = segments[textSegmentIndex];

            if (textSegment.Text.Equals("obo", StringComparison.InvariantCultureIgnoreCase))
                return new OrBestOfferSegment(textSegment.Text, textSegment.HasPrecedingSpace);
            else if (textSegment.Text.Equals("or", StringComparison.InvariantCultureIgnoreCase))
            {
                // STUB rewrite using segments and textSegmentIndex

                var nextSegment = GetNextSegment(segments, textSegmentIndex);
                if (nextSegment != null && nextSegment.GetType() == typeof(TextSegment) && nextSegment.Text.Equals("best", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nextNextSegment = GetNextSegment(segments, textSegmentIndex + 1);
                    if (nextNextSegment != null && nextNextSegment.GetType() == typeof(TextSegment) && nextNextSegment.Text.Equals("offer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        segments.Remove(nextNextSegment);
                        segments.Remove(nextSegment);
                        return new OrBestOfferSegment(textSegment.Text + ' ' + nextSegment.Text + ' ' + nextNextSegment.Text, textSegment.HasPrecedingSpace);
                    }
                }
            }

            return null;
        }


        // non-static


        // protected constructor so that these segments can only be created by calling TryParse()
        protected OrBestOfferSegment(string text, bool hasPrecedingSpace)
            : base(text, hasPrecedingSpace)
        {
        }
    }
}
