using System;
using System.Collections.Generic;
using System.Text;
using TunnelQuest.Data.Models;

namespace TunnelQuest.AppLogic.ChatSegments
{
    // Unlike all other segments, which preserve their original raw player-typed text, the entire purpose
    // of AuctionLinkSegment is to REPLACE its original player-typed text (or actually in 99% of cases,
    // the name of a player-linked item) with a special token string representing the item's AuctionId.

    internal class AuctionLinkSegment : BaseSegment
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

        public AuctionLinkSegment(ParsedChatLine parentLine, Auction auction)
            : base(parentLine, null)
        {
            this.Auction = auction;
        }

        public override string Text
        {
            get
            {
                return AUCTION_TOKEN + this._Auction.AuctionId.ToString();
            }
        }
    }
}
