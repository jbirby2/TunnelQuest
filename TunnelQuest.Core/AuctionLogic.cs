using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace TunnelQuest.Core
{
    public class AuctionLogic
    {
        // static stuff

        private static readonly TimeSpan MIN_NEW_AUCTION_THRESHOLD = TimeSpan.FromHours(1);
        private static readonly TimeSpan MAX_NEW_AUCTION_THRESHOLD = TimeSpan.FromHours(24);


        // non-static stuff

        private TunnelQuestContext context;

        public AuctionLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }

        public void UpdateReplacedAuctions(Auction newAuction)
        {
            

            // Set the IsPermanent property of the previous auction to False, occasionally leaving it as True to create
            // historical price history records.

            var mostRecentPermanentAuctionsBySamePlayerForSameItem = (from auction in context.Auctions
                                                                      where
                                                                        auction.ServerCode == newAuction.ServerCode
                                                                        && auction.ItemName == newAuction.ItemName
                                                                        && auction.PlayerName == newAuction.PlayerName
                                                                        && auction.IsPermanent == true
                                                                      orderby auction.CreatedAt descending
                                                                      select auction)
                                                                        .Take(2)
                                                                        .ToArray();

            if (mostRecentPermanentAuctionsBySamePlayerForSameItem.Length > 0)
            {
                Auction mostRecentAuction = mostRecentPermanentAuctionsBySamePlayerForSameItem[0];

                // set secondMostRecentAuction
                Auction secondMostRecentAuction;
                if (mostRecentPermanentAuctionsBySamePlayerForSameItem.Length > 1)
                    secondMostRecentAuction = mostRecentPermanentAuctionsBySamePlayerForSameItem[1];
                else
                {
                    // If there IS no second-most-recent permanent auction, then instead compare mostRecentAuction
                    // to the very first auction the player ever posted for this item.  Note this means that if this is the
                    // second time the player has ever auctioned this item, secondMostRecentAuction and mostRecentAuction 
                    // could end up being the same auction.
                    secondMostRecentAuction = (from auction in context.Auctions
                                               where
                                                  auction.ServerCode == newAuction.ServerCode
                                                  && auction.ItemName == newAuction.ItemName
                                                  && auction.PlayerName == newAuction.PlayerName
                                               orderby auction.CreatedAt ascending
                                               select auction)
                                                .FirstOrDefault();
                }

                newAuction.ReplacesAuctionId = mostRecentAuction.AuctionId;

                var newAuctionThreshold = (newAuction.Equals(mostRecentAuction) ? MAX_NEW_AUCTION_THRESHOLD : MIN_NEW_AUCTION_THRESHOLD);
                if (newAuction.CreatedAt - secondMostRecentAuction.CreatedAt < newAuctionThreshold)
                {
                    mostRecentAuction.IsPermanent = false;
                    mostRecentAuction.UpdatedAt = newAuction.CreatedAt;
                }
            }
        }

    }

}
