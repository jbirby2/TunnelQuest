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

        private static Node rootNode = new Node();
        private static readonly TimeSpan REFRESH_EXISTING_AUCTION_TIMEFRAME = TimeSpan.FromHours(24);

        // static constructor pulls a list of every item name and builds a big tree of letter paths for every possible name
        static AuctionLogic()
        {
            IEnumerable<string> itemNames;
            using (var context = new TunnelQuestContext())
            {
                itemNames = (from item in context.Items
                             orderby item.ItemName
                             select item.ItemName).ToArray();
            }

            foreach (string name in itemNames)
            {
                Node currentNode = rootNode;
                foreach (char letter in name)
                {
                    if (!currentNode.NextChars.ContainsKey(letter))
                        currentNode.NextChars[letter] = new Node();
                    currentNode = currentNode.NextChars[letter];
                }
                currentNode.ItemName = name;
            }
        }


        // non-static stuff

        private TunnelQuestContext context;

        internal AuctionLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }

        // called only by ChatLogic.ProcessLogLine()
        internal IEnumerable<Auction> GetAuctions(string serverCode, string playerName, string playerTypedText)
        {
            var auctions = new List<Auction>();

            var proposedNewAuctions = parseAuctions(playerTypedText);

            // We don't want to allow players to spam the database with tons of auctions by constantly re-posting the
            // same item with different auction details (e.g. price), so below we'll impose some logic to
            // make sure that a player can't create more than 1 auction for the same item within the timeframe
            // defined by REFRESH_EXISTING_AUCTION_TIMEFRAME.  If a player re-posts the same item with a different
            // price before the timeframe has passed, then we'll overwrite the details of their existing recent
            // auction for that item.
            foreach (Auction newAuction in proposedNewAuctions)
            {
                Auction lastAuctionBySamePlayerForSameItem = (from auction in context.Auctions
                                                              join chatLineAuction in context.ChatLineAuctions on auction.AuctionId equals chatLineAuction.AuctionId
                                                              join chatLine in context.ChatLines on chatLineAuction.ChatLineId equals chatLine.ChatLineId
                                                              where
                                                                chatLine.ServerCode == serverCode
                                                                && chatLine.PlayerName == playerName
                                                                && auction.ItemName == newAuction.ItemName
                                                              orderby auction.UpdatedAt descending
                                                              select auction).FirstOrDefault();

                if (lastAuctionBySamePlayerForSameItem == null)
                {
                    // Player has never auctioned this item before: create a new auction.
                    auctions.Add(newAuction);
                }
                else
                {
                    DateTime createNewAuctionDate = lastAuctionBySamePlayerForSameItem.CreatedAt + REFRESH_EXISTING_AUCTION_TIMEFRAME;

                    if (lastAuctionBySamePlayerForSameItem.Equals(newAuction))
                    {
                        // Player has auctioned this item before, and nothing has changed: reuse the existing auction.
                        lastAuctionBySamePlayerForSameItem.UpdatedAt = DateTime.UtcNow;
                        auctions.Add(lastAuctionBySamePlayerForSameItem);
                    }
                    else
                    {
                        // Player has auctioned this item before, and something HAS changed (price, etc)...

                        if (DateTime.UtcNow < createNewAuctionDate)
                        {
                            // It hasn't been long enough since the last time this player created a new auction
                            // for this item: update the existing auction with the new values.
                            lastAuctionBySamePlayerForSameItem.CopyValuesFrom(newAuction);
                            lastAuctionBySamePlayerForSameItem.UpdatedAt = DateTime.UtcNow;
                            auctions.Add(lastAuctionBySamePlayerForSameItem);
                        }
                        else
                        {
                            // It's been long enough since the last time this player created a new auction
                            // for this item: create a new auction, and leave the old auction as a historical record.
                            auctions.Add(newAuction);
                        }
                    }
                }
            }

            return auctions;
        }


        // private

        private IEnumerable<Auction> parseAuctions(string playerTypedText)
        {
            var auctions = new List<Auction>();


            // STUB TO DO - step through tokens and artificially add entries to itemNames for text that doesn't match known items.

            // STUB TO DO - make sure only 1 auction created for the same item per chat line, in case somebody includes it twice (even with different prices)

            var itemNames = parseItemNames(playerTypedText);

            /*
            // tokenize the longest names first, in case any of the item names start with the same text (e.g. "Broad Sword", "Broad Sword of Smiting")
            var sortedItemNames = itemNames.OrderByDescending(name => name.Length).ToArray();
            for (int i = 0; i < sortedItemNames.Length; i++)
            {
                playerTypedText = playerTypedText.Replace(sortedItemNames[i], ITEM_TOKEN_PREFIX + i.ToString());
            }
            */

            DateTime createdAt = DateTime.UtcNow;
            foreach (string itemName in itemNames)
            {
                auctions.Add(new Auction() {
                    ItemName = itemName,
                    IsBuying = false,           // STUB
                    Price = 123,                // STUB
                    IsPriceNegotiable = false,  // STUB
                    IsAcceptingTrades = false,   // STUB
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                });
            }

            return auctions;
        }

        private List<string> parseItemNames(string text)
        {
            var itemNames = new List<string>();

            int searchStartIndex = 0;
            while (searchStartIndex < text.Length)
            {
                Node prevNode = rootNode;
                var nodesTraversed = new Stack<Node>();
                int searchEndIndex = searchStartIndex;

                // build nodesTraversed
                while (searchEndIndex < text.Length)
                {
                    if (prevNode.NextChars.ContainsKey(text[searchEndIndex]))
                    {
                        prevNode = prevNode.NextChars[text[searchEndIndex]];
                        nodesTraversed.Push(prevNode);
                        searchEndIndex++;
                    }
                    else
                        break;
                }

                // now traverse backwards through nodesTraversed and find the deepest Node that had an itemName (if any)
                while (nodesTraversed.Count > 0)
                {
                    var lastNode = nodesTraversed.Pop();
                    if (lastNode.ItemName != null)
                    {
                        if (!itemNames.Contains(lastNode.ItemName))
                            itemNames.Add(lastNode.ItemName);
                        break;
                    }
                }

                searchStartIndex += nodesTraversed.Count + 1;
            }

            return itemNames;
        }

        // helper class
        private class Node
        {
            public string ItemName { get; set; } = null;
            public Dictionary<char, Node> NextChars { get; } = new Dictionary<char, Node>();
        }
    }
}
