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

        public string[] GetAllItemNames(string serverCode, bool includeBuying, bool includeUnpriced)
        {
            var auctionQuery = context.Auctions
                .Include(auction => auction.MostRecentChatLine)
                    .ThenInclude(chatLine => chatLine.Tokens)
                        .ThenInclude(chatLineToken => chatLineToken.Properties)
                .Where(auction => auction.MostRecentChatLine.ServerCode == serverCode);

            if (!includeBuying)
                auctionQuery = auctionQuery.Where(auction => auction.IsBuying == false);

            if (!includeUnpriced)
                auctionQuery = auctionQuery.Where(auction => auction.Price != null && auction.Price > 0);

            return auctionQuery
                .Select(auction => auction.ItemName)
                .Distinct()
                .OrderBy(itemName => itemName)
                .ToArray();
        }

        public Auction[] GetAuctions(AuctionsQuery criteria)
        {
            IQueryable<Auction> auctionQuery;
            if (criteria.IncludeChatLine)
            {
                auctionQuery = context.Auctions
                    .Include(auction => auction.MostRecentChatLine)
                        .ThenInclude(chatLine => chatLine.Tokens)
                                .ThenInclude(chatLineToken => chatLineToken.Properties)
                    .Where(auction => auction.MostRecentChatLine.ServerCode == criteria.ServerCode);
            }
            else
            {
                auctionQuery = context.Auctions
                    .Where(auction => auction.MostRecentChatLine.ServerCode == criteria.ServerCode);
            }

            if (!String.IsNullOrWhiteSpace(criteria.ItemName))
                auctionQuery = auctionQuery.Where(auction => auction.ItemName.Equals(criteria.ItemName, StringComparison.InvariantCultureIgnoreCase));

            if (!criteria.IncludeBuying)
                auctionQuery = auctionQuery.Where(auction => auction.IsBuying == false);

            if (!criteria.IncludeUnpriced)
                auctionQuery = auctionQuery.Where(auction => auction.Price != null && auction.Price > 0);

            if (criteria.MinimumId != null)
                auctionQuery = auctionQuery.Where(auction => auction.AuctionId >= criteria.MinimumId.Value);

            if (criteria.MaximumId != null)
                auctionQuery = auctionQuery.Where(auction => auction.AuctionId <= criteria.MaximumId.Value);


            // order by descending in the sql query, to make sure we get the most recent auctions if we hit the limit imposed by maxResults
            auctionQuery = auctionQuery.OrderByDescending(auction => auction.UpdatedAt);

            if (criteria.MaxResults != null)
                auctionQuery = auctionQuery.Take(criteria.MaxResults.Value);

            return auctionQuery
                .ToArray() // call .ToArray() to force entity framework to execute the query and get the results from the database
                .OrderBy(auction => auction.UpdatedAt) // now that we've got the results from the database (possibly truncated by maxResults), re-order them correctly
                .ToArray();
        }

        public Dictionary<string, Auction> GetNormalizedAuctions(string serverCode, string playerName, DateTime timestamp, Dictionary<string, Auction> pendingAuctions)
        {
            // It's possible that, for whatever reason, the same item will be listed in a chat line more than once.  When that happens,
            // we'll apply some logic to decide which auction is more "correct", and make all the AuctionSegments point at that same Auction object.
            var normalizedAuctions = new Dictionary<string, Auction>();
            foreach (var pendingAuction in pendingAuctions.Values)
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

                // See if there's an existing auction we should reuse instead of posting this new one.

                Auction lastAuctionBySamePlayerForSameItem = (from auction in context.Auctions
                                                              join chatLine in context.ChatLines on auction.MostRecentChatLineId equals chatLine.ChatLineId
                                                              where
                                                                chatLine.ServerCode == serverCode
                                                                && chatLine.PlayerName == playerName
                                                                && auction.ItemName == pendingNewAuction.ItemName
                                                              orderby auction.UpdatedAt descending
                                                              select auction).FirstOrDefault();

                if (lastAuctionBySamePlayerForSameItem != null)
                {
                    if (lastAuctionBySamePlayerForSameItem.Equals(pendingNewAuction))
                    {
                        // Player has auctioned this item before, and nothing has changed...

                        DateTime createNewAuctionDate = lastAuctionBySamePlayerForSameItem.CreatedAt + MAX_NEW_AUCTION_THRESHOLD;

                        if (DateTime.UtcNow < createNewAuctionDate)
                        {
                            // reuse the existing auction.
                            lastAuctionBySamePlayerForSameItem.UpdatedAt = timestamp;
                            normalizedAuctions[itemName] = lastAuctionBySamePlayerForSameItem;
                        }
                        else
                        {
                            // create a new auction (even though nothing has changed) so that the old auction
                            // can become historical data for reporting
                            pendingNewAuction.PreviousAuctionId = lastAuctionBySamePlayerForSameItem.AuctionId;
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
                            normalizedAuctions[itemName] = lastAuctionBySamePlayerForSameItem;
                        }
                        else
                        {
                            // It's been long enough since the last time this player created a new auction
                            // for this item: create a new auction, and leave the old auction as a historical record.
                            pendingNewAuction.PreviousAuctionId = lastAuctionBySamePlayerForSameItem.AuctionId;
                        }
                    }
                }
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


    } // end class

}
