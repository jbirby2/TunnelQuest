using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TunnelQuest.Data.Models;
using TunnelQuest.AppLogic.ChatSegments;

namespace TunnelQuest.AppLogic
{
    internal class ParsedChatLine
    {
        public string PlayerName { get; private set; }
        public string PlayerTypedText { get; private set; }
        public List<BaseSegment> Segments { get; private set; }

        // Timestamp is used to set the CreatedAt and UpdatedAt columns for any database objects.
        // If I just set them each to DateTime.UtcNow individually, then they might end up
        // with slightly different values.  By setting them all from the same Timestamp
        // value instead, I make sure that all child records always have the exact same
        // CreatedAt value as their parent record (and each other).
        public DateTime Timestamp { get; private set; }

        public ParsedChatLine(string logLine)
        {
            this.Timestamp = DateTime.UtcNow;

            this.Segments = new List<BaseSegment>();

            string[] lineWords = logLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            try
            {
                if (lineWords.Length < 8 || lineWords[0][0] != '[' || lineWords[4][4] != ']' || lineWords[6] != "auctions,")
                    throw new InvalidLogLineException(logLine);
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidLogLineException(logLine);
            }

            this.PlayerName = lineWords[5];
            this.PlayerTypedText = String.Join(' ', lineWords, 7, lineWords.Length - 7).Trim('\'');
        }

        public IEnumerable<Auction> GetAuctions()
        {
            var auctions = new List<Auction>();
            foreach (BaseSegment segment in this.Segments)
            {
                if (segment is AuctionLinkSegment)
                {
                    var auction = ((AuctionLinkSegment)segment).Auction;
                    if (!auctions.Contains(auction))
                        auctions.Add(auction);
                }
            }
            return auctions;
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < this.Segments.Count; i++)
            {
                if (i > 0)
                    str += " ";
                str += this.Segments[i].Text;
            }
            return str;
        }

        public BaseSegment NextSegment(BaseSegment segment)
        {
            if (!Segments.Contains(segment))
                throw new Exception("this.Segments does not contain parameter segment");

            int index = this.Segments.IndexOf(segment);
            if (index + 1 < this.Segments.Count)
                return this.Segments[index + 1];
            else
                return null;
        }

        public BaseSegment PrevSegment(BaseSegment segment)
        {
            if (!Segments.Contains(segment))
                throw new Exception("this.Segments does not contain parameter segment");

            int index = this.Segments.IndexOf(segment);
            if (index - 1 > 0)
                return this.Segments[index - 1];
            else
                return null;
        }
    }
}
