using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Data.Models;

namespace TunnelQuest.AppLogic.ChatSegments
{
    internal class AuctionSegment : ChatSegment
    {
        // static

        public static readonly string AUCTION_TOKEN = "#TQAUC_";


        // non-static

        private Auction _Auction;
        public Auction Auction
        {
            get { return _Auction; }
            set
            {
                if (value == null)
                    throw new Exception("Auction cannot be set null");
                _Auction = value;
            }
        }

        public AuctionSegment(Auction auction)
            : base(null)
        {
            this.Auction = auction;
        }

        // Unlike other segments which preserve their original raw player-typed text, the AuctionSegments
        // actually want to REPLACE the original player-typed text with a token that identifies the saved Auction record
        public override string Text
        {
            get
            {
                return AUCTION_TOKEN + this._Auction.AuctionId.ToString();
            }
        }
    }
}
