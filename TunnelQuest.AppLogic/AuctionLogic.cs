using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;
using TunnelQuest.AppLogic.ChatSegments;

namespace TunnelQuest.AppLogic
{
    public class AuctionLogic
    {
        public const int MAX_AUCTIONS = 1000;


        // static stuff

        private static readonly TimeSpan MIN_NEW_AUCTION_THRESHOLD = TimeSpan.FromHours(1);
        private static readonly TimeSpan MAX_NEW_AUCTION_THRESHOLD = TimeSpan.FromHours(24);


        // non-static stuff

        private TunnelQuestContext context;

        internal AuctionLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }

        public Dictionary<string, Auction> GetNormalizedAuctions(string serverCode, string playerName, DateTime timestamp, IEnumerable<Auction> pendingAuctions)
        {
            // It's possible that, for whatever reason, the same item will be listed in a chat line more than once.  When that happens,
            // we'll apply some logic to decide which auction is more "correct", and make all the AuctionSegments point at that same Auction object.
            var normalizedAuctions = new Dictionary<string, Auction>();
            foreach (var pendingAuction in pendingAuctions)
            {
                if (normalizedAuctions.ContainsKey(pendingAuction.ItemName))
                    normalizedAuctions[pendingAuction.ItemName] = whichAuctionIsMoreComplete(pendingAuction, normalizedAuctions[pendingAuction.ItemName]);
                else
                    normalizedAuctions[pendingAuction.ItemName] = pendingAuction;
            }

            // For each auction that the player is advertising, check to see if it's a re-post of an
            // existing auction.  This will be true 99% of the time.
            foreach (var itemName in normalizedAuctions.Keys.ToArray())
            {
                var pendingNewAuction = normalizedAuctions[itemName];

                // See if there's an existing auction we should reuse instead of posting this new one.  If so, use it instead of the pending new auction.
                Auction auctionToReuse = getReusableAuction(serverCode, playerName, timestamp, pendingNewAuction);
                if (auctionToReuse != null)
                    normalizedAuctions[itemName] = auctionToReuse;
            }

            return normalizedAuctions;
        }


        // private

        private Auction whichAuctionIsMoreComplete(Auction auction1, Auction auction2)
        {
            int a1Score = 0;
            int a2Score = 0;

            if (auction1.IsAcceptingTrades == true && auction2.IsAcceptingTrades == false)
                a1Score++;
            else if (auction1.IsAcceptingTrades == false && auction2.IsAcceptingTrades == true)
                a2Score++;

            if (auction1.IsOrBestOffer == true && auction2.IsOrBestOffer == false)
                a1Score++;
            else if (auction1.IsOrBestOffer == false && auction2.IsOrBestOffer == true)
                a2Score++;

            if (auction1.Price != null && auction2.Price == null)
                a1Score++;
            else if (auction1.Price == null && auction2.Price != null)
                a2Score++;
            else if (auction1.Price != null && auction2.Price != null)
            {
                if (auction1.Price > 0 && auction2.Price <= 0)
                    a1Score++;
                else if (auction1.Price <= 0 && auction2.Price > 0)
                    a2Score++;
            }

            if (a2Score > a1Score)
                return auction2;
            else
                return auction1;
        }

        // Returns an Auction if there's an existing Auction which can be reused instead of pendingAuction, based on
        // the rules defined within the function.  If there is no existing auction that we want to reuse, then return
        // null.
        private Auction getReusableAuction(string serverCode, string playerName, DateTime timestamp, Auction pendingNewAuction)
        {
            Auction lastAuctionBySamePlayerForSameItem = (from auction in context.Auctions
                                                          join chatLineAuction in context.ChatLineAuctions on auction.AuctionId equals chatLineAuction.AuctionId
                                                          join chatLine in context.ChatLines on chatLineAuction.ChatLineId equals chatLine.ChatLineId
                                                          where
                                                            chatLine.ServerCode == serverCode
                                                            && chatLine.PlayerName == playerName
                                                            && auction.ItemName == pendingNewAuction.ItemName
                                                          orderby auction.UpdatedAt descending
                                                          select auction).FirstOrDefault();

            if (lastAuctionBySamePlayerForSameItem == null)
            {
                // Player has never auctioned this item before: create a new auction.
                return null;
            }
            else
            {
                if (lastAuctionBySamePlayerForSameItem.Equals(pendingNewAuction))
                {
                    // Player has auctioned this item before, and nothing has changed...

                    DateTime createNewAuctionDate = lastAuctionBySamePlayerForSameItem.CreatedAt + MAX_NEW_AUCTION_THRESHOLD;

                    if (DateTime.UtcNow < createNewAuctionDate)
                    {
                        // reuse the existing auction.
                        return lastAuctionBySamePlayerForSameItem;
                    }
                    else
                    {
                        // create a new auction (even though nothing has changed) so that the old auction
                        // can become historical data for reporting
                        return null;
                    }
                }
                else
                {
                    // Player has auctioned this item before, and something HAS changed (price, etc)...

                    DateTime createNewAuctionDate = lastAuctionBySamePlayerForSameItem.CreatedAt + MIN_NEW_AUCTION_THRESHOLD;

                    if (DateTime.UtcNow < createNewAuctionDate)
                    {
                        // It hasn't been long enough since the last time this player created a new auction
                        // for this item: update the existing auction with the new values.
                        lastAuctionBySamePlayerForSameItem.CopyValuesFrom(pendingNewAuction);
                        lastAuctionBySamePlayerForSameItem.UpdatedAt = timestamp;
                        return lastAuctionBySamePlayerForSameItem;
                    }
                    else
                    {
                        // It's been long enough since the last time this player created a new auction
                        // for this item: create a new auction, and leave the old auction as a historical record.
                        return null;
                    }
                }
            }

        } // end function
    
    } // end class

}
