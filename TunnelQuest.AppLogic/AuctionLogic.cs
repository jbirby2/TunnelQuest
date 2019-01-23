using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;
using TunnelQuest.AppLogic.ChatSegments;
using Microsoft.EntityFrameworkCore;

namespace TunnelQuest.AppLogic
{
    public class AuctionLogic
    {
        public const int MAX_AUCTIONS = 100;


        // static stuff

        private static readonly TimeSpan MIN_AUCTION_HISTORY_THRESHOLD = TimeSpan.FromHours(1);
        private static readonly TimeSpan MAX_AUCTION_HISTORY_THRESHOLD = TimeSpan.FromHours(24);


        // non-static stuff

        private TunnelQuestContext context;

        public AuctionLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }

        public Auction[] GetAuctions(string serverCode, DateTime? minUpdatedAt = null, DateTime? maxUpdatedAt = null, int? maxResults = null)
        {
            if (maxResults == null)
                maxResults = MAX_AUCTIONS;

            var auctionQuery = context.Auctions
               .Include(auction => auction.ChatLines)
                   .ThenInclude(auctionChatLine => auctionChatLine.ChatLine)
                       .ThenInclude(chatLine => chatLine.Auctions)
                           .ThenInclude(chatLineAuction => chatLineAuction.Auction)
                .Where(auction => auction.ChatLines.Any(auctionChat => auctionChat.ChatLine.ServerCode == serverCode));
            
            if (minUpdatedAt != null)
                auctionQuery = auctionQuery.Where(auction => auction.UpdatedAt >= minUpdatedAt.Value);

            if (maxUpdatedAt != null)
                auctionQuery = auctionQuery.Where(auction => auction.UpdatedAt <= maxUpdatedAt.Value);

            // STUB TODO: come back and rewrite this more efficiently.
            //
            // This query sucks because it returns ALL associated rows from ChatLineAuction, and THEN discards
            // all but the most recent ChatLineAuction row.  I got tired of trying to figure out how to make
            // Entity Framework select the parent record + only the most recent child record, AND its associated ChatLine record.
            // Luckily the SQL Server is running on the same machine as the web server, so we can probably get away with this
            // shameful wastefulness.

            var auctions = auctionQuery
                .OrderByDescending(auction => auction.UpdatedAt) // order by descending in the sql query, to make sure we get the most recent auctions if we hit the limit imposed by maxResults
                .Take(maxResults.Value)
                .ToArray() // call .ToArray() to force entity framework to execute the query and get the results from the database
                .OrderBy(auction => auction.UpdatedAt) // now that we've got the results from the database (possibly truncated by maxResults), re-order them correctly
                .ToArray();

            // now discard all but the most recent chat line
            foreach (var auction in auctions)
            {
                var lastChatLine = auction.ChatLines.LastOrDefault();
                var newCollection = new List<ChatLineAuction>();
                if (lastChatLine != null)
                    newCollection.Add(lastChatLine);
                auction.ChatLines = newCollection;
            }

            return auctions;
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
                // Player has auctioned this item before: re-use the existing auction, but copy over the values
                // from the new chat line
                lastAuctionBySamePlayerForSameItem.CopyValuesFrom(pendingNewAuction);
                lastAuctionBySamePlayerForSameItem.UpdatedAt = timestamp;
                return lastAuctionBySamePlayerForSameItem;
            }

        } // end function
    
    } // end class

}
