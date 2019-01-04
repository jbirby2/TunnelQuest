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
        public List<ChatSegment> Segments { get; private set; }

        public ParsedChatLine(string playerName, List<ChatSegment> segments)
        {
            this.PlayerName = playerName;
            this.Segments = segments;
        }

        public IEnumerable<Auction> GetAuctions()
        {
            var auctions = new List<Auction>();
            foreach (ChatSegment segment in this.Segments)
            {
                if (segment is AuctionSegment)
                {
                    var auction = ((AuctionSegment)segment).Auction;
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
    }
}
