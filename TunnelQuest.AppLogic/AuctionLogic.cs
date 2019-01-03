using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;

namespace TunnelQuest.AppLogic
{
    internal class AuctionLogic
    {
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

        public void ApplyNewAuctionRules(string serverCode, ParsedChatLine parsedLine)
        {
            var auctionsToUse = new Dictionary<string, Auction>();
            foreach (var newAuction in parsedLine.GetAuctions())
            {
                Auction auctionToUse = findExistingAuction(serverCode, parsedLine.PlayerName, newAuction);
                if (auctionToUse == null)
                    auctionToUse = newAuction;
                auctionsToUse.Add(auctionToUse.ItemName, auctionToUse);
            }

            // update every AuctionWord in parsedLine to make sure it's referencing the correct version of its auction
            foreach (var chatWord in parsedLine.Words)
            {
                if (chatWord is AuctionWord)
                {
                    var auctionWord = (AuctionWord)chatWord;
                    auctionWord.Auction = auctionsToUse[auctionWord.Auction.ItemName];
                }
            }
        }


        // private

        private Auction findExistingAuction(string serverCode, string playerName, Auction pendingAuction)
        {
            Auction lastAuctionBySamePlayerForSameItem = (from auction in context.Auctions
                                                          join chatLineAuction in context.ChatLineAuctions on auction.AuctionId equals chatLineAuction.AuctionId
                                                          join chatLine in context.ChatLines on chatLineAuction.ChatLineId equals chatLine.ChatLineId
                                                          where
                                                            chatLine.ServerCode == serverCode
                                                            && chatLine.PlayerName == playerName
                                                            && auction.ItemName == pendingAuction.ItemName
                                                          orderby auction.UpdatedAt descending
                                                          select auction).FirstOrDefault();

            if (lastAuctionBySamePlayerForSameItem == null)
            {
                // Player has never auctioned this item before: create a new auction.
                return null;
            }
            else
            {
                if (lastAuctionBySamePlayerForSameItem.Equals(pendingAuction))
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
                        lastAuctionBySamePlayerForSameItem.CopyValuesFrom(pendingAuction);
                        lastAuctionBySamePlayerForSameItem.UpdatedAt = DateTime.UtcNow;
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
