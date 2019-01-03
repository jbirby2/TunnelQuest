using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Data.Models;

namespace TunnelQuest.AppLogic
{
    internal class AuctionWord : ChatWord
    {
        // static

        public static readonly string AUCTION_TOKEN = "#TQAUC_";

        // non-static

        public Auction Auction { get; set; }

        public AuctionWord(Auction auction)
            : base(null)
        {
            if (auction == null)
                throw new Exception("auction cannot be null");

            this.Auction = auction;
        }

        public override string ToString()
        {
            return AUCTION_TOKEN + this.Auction.AuctionId.ToString();
        }
    }
}
