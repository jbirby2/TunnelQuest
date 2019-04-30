using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;
using System.Threading;
using TunnelQuest.Core.ChatSegments;
using System.Runtime.Caching;
using Microsoft.EntityFrameworkCore;

namespace TunnelQuest.Core
{
    public class ChatLogic
    {
        public const string CHAT_TOKEN = "#TQT#";
        public const int MAX_CHAT_LINES = 100;

        // static stuff

        private static readonly Dictionary<string, short> tokenCache = new Dictionary<string, short>();
        private static readonly object CHAT_LINE_LOCK = new object();
        private static readonly TimeSpan DUPLICATE_LINE_FILTER_THRESHOLD = TimeSpan.FromSeconds(10);
        private static Node itemNamesRootNode = null;

        // static constructor
        static ChatLogic()
        {
            // get the list of all item names
            IEnumerable<string> itemNames;
            IEnumerable<Alias> itemAliases;
            using (var context = new TunnelQuestContext())
            {
                itemNames = (from item in context.Items
                             orderby item.ItemName
                             select item.ItemName).ToArray();

                itemAliases = (from alias in context.Aliases
                               orderby alias.AliasText
                               select alias).ToArray();
            }

            // build itemNamesRootNode
            itemNamesRootNode = new Node();
            foreach (Alias alias in itemAliases)
            {
                Node currentNode = itemNamesRootNode;
                foreach (char letter in alias.AliasText.ToLower()) // ToLower() because we want all matching to be case-insensitive
                {
                    if (!currentNode.NextChars.ContainsKey(letter))
                        currentNode.NextChars[letter] = new Node();
                    currentNode = currentNode.NextChars[letter];
                }
                currentNode.ItemName = alias.ItemName;
            }
            foreach (string name in itemNames)
            {
                Node currentNode = itemNamesRootNode;
                foreach (char letter in name.ToLower()) // ToLower() because we want all matching to be case-insensitive
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
        public ChatLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }

        public ChatLinesQueryResult GetLines(ChatLinesQuery criteria)
        {
            // Only reference columns from the auction table in our .Where() clauses so that auction table indexes will be used.
            // Also remember that the order of the where clauses has to match the order of the columns in the index.


            var auctionQuery = context.Auctions
                .Where(auction => auction.ServerCode == criteria.ServerCode);

            if (criteria.MinimumId != null)
                auctionQuery = auctionQuery.Where(auction => auction.ChatLineId >= criteria.MinimumId.Value);

            if (criteria.MaximumId != null)
                auctionQuery = auctionQuery.Where(auction => auction.ChatLineId <= criteria.MaximumId.Value);

            if (criteria.FilterSettings != null)
            {
                if (criteria.FilterSettings.Items.FilterType == "name" && criteria.FilterSettings.Items.Names.Any())
                    auctionQuery = auctionQuery.Where(auction => criteria.FilterSettings.Items.Names.Contains(auction.ItemName));

                if (criteria.FilterSettings.IsBuying != null)
                    auctionQuery = auctionQuery.Where(auction => auction.IsBuying == criteria.FilterSettings.IsBuying.Value);

                if (criteria.FilterSettings.PlayerName != null)
                    auctionQuery = auctionQuery.Where(auction => auction.PlayerName == criteria.FilterSettings.PlayerName);

                if (criteria.FilterSettings.IsPermanent != null)
                    auctionQuery = auctionQuery.Where(auction => auction.IsPermanent == criteria.FilterSettings.IsPermanent.Value);

                if (criteria.FilterSettings.Items.FilterType == "stats")
                {
                    var auctionItemQuery = auctionQuery
                        .Where(auction => auction.IsKnownItem == true)
                        .Join(context.Items,
                            auction => auction.ItemName,
                            item => item.ItemName,
                            (auction, item) => new { Auction = auction, Item = item })
                        .Where(auctionItem => auctionItem.Item != null); // technically this null check shouldn't be necessary since we're already filtering isKnownItem = true

                    if (criteria.FilterSettings.Items.Stats.MinStrength != null)
                        auctionItemQuery = auctionItemQuery.Where(auctionItem => auctionItem.Item.Strength != null && auctionItem.Item.Strength.Value >= criteria.FilterSettings.Items.Stats.MinStrength.Value);

                    // ... STUB other filters go here

                    auctionQuery = auctionItemQuery.Select(auctionItem => auctionItem.Auction);
                }

                // STUB TODO - price filtering and price deviation filtering
            }

            // Execute auctionQuery and get the list of matching AuctionIds back from the database.
            var filteredAuctionIds = auctionQuery.Select(auction => auction.AuctionId).ToHashSet();
            
            // Build a second query that will get the ChatLines based on the filteredAuctionIds from the first query.
            // The reason for using two queries is so that we can use filteredAuctionIds to set ClientAuction.PassesFilter
            // later on.
            var chatQuery = context.ChatLines
                .Include(chatLine => chatLine.Auctions)
                .Where(chatLine => chatLine.Auctions.Any(auction => filteredAuctionIds.Contains(auction.AuctionId)))
                .OrderByDescending(line => line.ChatLineId) // order by descending in the sql query, to make sure we get the most recent lines if we hit the limit imposed by maxResults
                .Distinct();

            if (criteria.MaxResults != null)
                chatQuery = chatQuery.Take(criteria.MaxResults.Value);

            var chatLines = chatQuery.ToArray() // call .ToArray() to force entity framework to execute the query and get the results from the database
                .OrderBy(line => line.ChatLineId) // now that we've got the results from the database (possibly truncated by maxResults), re-order them correctly
                .ToArray();

            return new ChatLinesQueryResult()
            {
                ChatLines = chatLines,
                FilteredAuctionIds = filteredAuctionIds
            };

        }

        public ChatLine ProcessLogLine(string authTokenValue, string serverCode, string logLine, DateTime timeStamp)
        {
            if (String.IsNullOrWhiteSpace(authTokenValue))
                throw new Exception("authTokenValue cannot be empty");

            if (String.IsNullOrWhiteSpace(logLine))
                throw new Exception("logLine cannot be empty");

            serverCode = ServerCodes.All.Where(cleanCode => cleanCode.Equals(serverCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(serverCode))
                throw new Exception("serverCode cannot be empty");

            Monitor.Enter(CHAT_LINE_LOCK);
            try
            {
                short authTokenId;
                if (!tokenCache.ContainsKey(authTokenValue))
                {
                    authTokenId = (from token in context.AuthTokens
                                   where token.Value == authTokenValue && token.AuthTokenStatusCode == AuthTokenStatusCodes.Approved
                                   select token.AuthTokenId).FirstOrDefault();

                    if (authTokenId <= 0)
                        throw new InvalidAuthTokenException();
                    else
                        tokenCache[authTokenValue] = authTokenId;
                }
                else
                    authTokenId = tokenCache[authTokenValue];
                

                // save everything to database
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // remember that the auctions get saved to the database inside of parseChatLine()
                        var parseResult = parseChatLine(tokenCache[authTokenValue], serverCode, logLine, timeStamp, context);

                        // due to the nature of the circular references between chat_line and auction, we have no choice
                        // but to first insert the rows to create their ids, and then update chat_line.text as a second
                        // operation (because it needs the auction ids in its chat tokens)
                        parseResult.ChatLine.Text = "";
                        context.Add(parseResult.ChatLine);
                        context.SaveChanges();

                        // now that the auctions have been inserted into the database and we have their final AuctionIds to use in chat tokens,
                        // we can build the chat text
                        parseResult.ChatLine.Text = "";
                        foreach (var segment in parseResult.Segments)
                        {
                            if (segment.HasPrecedingSpace)
                                parseResult.ChatLine.Text += ' ';
                            parseResult.ChatLine.Text += segment.Text;
                        }

                        // also update the unknown items table (if necessary)
                        foreach (var auction in parseResult.ChatLine.Auctions.Where(auction => auction.IsKnownItem == false))
                        {
                            if (context.UnknownItems.Count(unknownItem => unknownItem.ServerCode == serverCode && unknownItem.IsBuying == auction.IsBuying && unknownItem.ItemName == auction.ItemName) == 0)
                            {
                                context.UnknownItems.Add(new UnknownItem()
                                {
                                    ServerCode = serverCode,
                                    IsBuying = auction.IsBuying,
                                    ItemName = auction.ItemName,
                                    CreatedAt = auction.CreatedAt
                                });
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();

                        return parseResult.ChatLine;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Monitor.Exit(CHAT_LINE_LOCK);
            }
        }

        private ParseResult parseChatLine(short authTokenId, string serverCode, string logLine, DateTime timeStamp, TunnelQuestContext context)
        {
            var newLine = new ChatLine();
            newLine.AuthTokenId = authTokenId;
            newLine.ServerCode = serverCode;
            newLine.SentAt = timeStamp;

            string[] lineWords = logLine.Split(' ', StringSplitOptions.None);

            try
            {
                if (lineWords.Length < 3 || lineWords[1] != "auctions,")
                    throw new InvalidLogLineException(logLine);
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidLogLineException(logLine);
            }

            newLine.PlayerName = lineWords[0];
            string playerTypedText = String.Join(' ', lineWords, 2, lineWords.Length - 2).Trim('\'');

            var segments = parseItemNames(playerTypedText, timeStamp);

            // At this point, the only types of segments in Segments are TextSegments and ItemNameSegments.  We want
            // to loop through and attempt to replace each of the generic TextSegments (which represent unrecognized text)
            // with more specific Segments which represent recognized data elements.

            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                if (segment.GetType() == typeof(TextSegment))
                {
                    if (segment.Text.Contains(ChatLogic.CHAT_TOKEN))
                    {
                        // in case anybody tries to be mischevious and actually type the token string into chat
                        segments[i] = new TextSegment("clever girl", segment.HasPrecedingSpace);
                    }
                    else
                    {
                        TextSegment parsedSegment = PriceSegment.TryParse(segments, i);

                        if (parsedSegment == null)
                            parsedSegment = SeparatorSegment.TryParse(segment);

                        if (parsedSegment == null)
                            parsedSegment = BuySellTradeSegment.TryParse(segment);

                        if (parsedSegment == null)
                            parsedSegment = OrBestOfferSegment.TryParse(segments, i);

                        if (parsedSegment != null)
                            segments[i] = parsedSegment;
                    }
                }
            }

            // Now that we've parsed all the segments we can recognize, go back through  and see if we 
            // can intuit any non-linked auctions (e.g. "WTS jboots mq 5k")

            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i].GetType() == typeof(TextSegment))
                {
                    // First, merge this TextSegment with any other TextSegments that come immediately after it
                    var indexesToMerge = new List<int>();
                    indexesToMerge.Add(i);
                    string mergedText = segments[i].Text;
                    bool mergedTextHasPrecedingSpace = segments[i].HasPrecedingSpace;
                    for (int j = i + 1; j < segments.Count; j++)
                    {
                        if (segments[j].GetType() == typeof(TextSegment))
                        {
                            indexesToMerge.Add(j);

                            if (segments[j].HasPrecedingSpace)
                                mergedText += ' ';
                            mergedText += segments[j].Text;
                        }
                        else
                            break;
                    }
                    indexesToMerge.Reverse(); // delete indexes from back-to-front so that each deleted index doesn't shift the remaining ones into new positions
                    foreach (int index in indexesToMerge)
                    {
                        segments.RemoveAt(index);
                    }

                    // Now that we've got a string containing the text of all the adjacent TextSegments, decide
                    // whether it's something we should create an auction for
                    bool createAuction;

                    // Assume that any unrecognized text which comes *immediately* before or after after an item link is a 
                    // description of the item, and *not* something we should create an auction for.
                    if (i > 0 && segments[i - 1] is ItemNameSegment)
                        createAuction = false;
                    else if (i < segments.Count && segments[i] is ItemNameSegment)
                        createAuction = false;
                    else if (mergedText.StartsWith("PST", StringComparison.InvariantCultureIgnoreCase))
                        createAuction = false;
                    else
                        createAuction = true;

                    // STUB TODO - will probably end up adding more fine-tuned logic here after testing more
                    // real world auction logs

                    if (createAuction)
                        segments.Insert(i, new ItemNameSegment(mergedText, mergedText.Trim(), false, mergedTextHasPrecedingSpace)); // the .Trim() is important
                    else
                        segments.Insert(i, new TextSegment(mergedText, mergedTextHasPrecedingSpace));
                }
            }

            // Now that we've created all of the ItemNameSegments, loop through all the segments one last time and
            // use their values to build the Auction objects.  If the same item name is found more than once, only create
            // one single auction for the item.

            var auctions = new Dictionary<string, Auction>();
            BuySellTradeSegment lastFoundBuySellTrade = null;
            var itemsSinceLastSeparator = new List<ItemNameSegment>();
            
            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];

                if (segment is ItemNameSegment)
                {
                    var itemNameSegment = (ItemNameSegment)segment;

                    itemsSinceLastSeparator.Add(itemNameSegment);

                    Auction auction;
                    if (auctions.ContainsKey(itemNameSegment.ItemName))
                    {
                        auction = auctions[itemNameSegment.ItemName];
                    }
                    else
                    {
                        auction = new Auction()
                        {
                            ChatLine = newLine,
                            ChatLineId = newLine.ChatLineId,
                            ServerCode = newLine.ServerCode,
                            PlayerName = newLine.PlayerName,
                            ItemName = itemNameSegment.ItemName,
                            AliasText = itemNameSegment.AliasText,
                            IsKnownItem = itemNameSegment.IsKnownItem,
                            IsPermanent = true,
                            CreatedAt = newLine.SentAt
                        };
                        newLine.Auctions.Add(auction);

                        auctions.Add(itemNameSegment.ItemName, auction);
                    }

                    itemNameSegment.Auction = auction;

                    if (lastFoundBuySellTrade != null)
                    {
                        if (lastFoundBuySellTrade.IsBuying != null)
                            auction.IsBuying = lastFoundBuySellTrade.IsBuying.Value;
                        if (lastFoundBuySellTrade.IsAcceptingTrades != null)
                            auction.IsAcceptingTrades = lastFoundBuySellTrade.IsAcceptingTrades.Value;
                    }
                }
                else if (segment is BuySellTradeSegment)
                {
                    lastFoundBuySellTrade = ((BuySellTradeSegment)segment);
                }
                else if (segment is PriceSegment)
                {
                    var priceSegment = (PriceSegment)segment;

                    var itemSegmentIndexesWithThisPrice = new List<int>();
                    foreach (var itemNameSegment in itemsSinceLastSeparator)
                    {
                        var auction = auctions[itemNameSegment.ItemName];

                        if (auction.Price == null)
                            auction.Price = priceSegment.Price;
                    }
                }
                else if (segment is OrBestOfferSegment)
                {
                    foreach (var itemNameSegment in itemsSinceLastSeparator)
                    {
                        var auction = auctions[itemNameSegment.ItemName];

                        if (auction.Price != null)
                            auction.IsOrBestOffer = true;
                    }
                }
                else if (segment is SeparatorSegment)
                {
                    itemsSinceLastSeparator.Clear();
                }
            }

            // now that all the segments have been evaluated and the Auctions contain their final values,
            // update any previously existing auctions as necessary
            var auctionLogic = new AuctionLogic(context);
            foreach (var auction in newLine.Auctions)
            {
                auctionLogic.UpdateReplacedAuctions(auction);
            }

            return new ParseResult()
            {
                ChatLine = newLine,
                Segments = segments
            };
        }

        private List<TextSegment> parseItemNames(string playerTypedText, DateTime timestamp)
        {
            var segments = new List<TextSegment>();
            string lowerPlayerTypedText = playerTypedText.ToLower();

            int searchStartIndex = 0;
            string currentSegmentText = "";
            bool wasPrecedingSpace = false;
            while (searchStartIndex < lowerPlayerTypedText.Length)
            {
                Node prevNode = itemNamesRootNode;
                var nodesTraversed = new Stack<Node>();
                int searchEndIndex = searchStartIndex;

                // build nodesTraversed
                while (searchEndIndex < lowerPlayerTypedText.Length)
                {
                    if (prevNode.NextChars.ContainsKey(lowerPlayerTypedText[searchEndIndex]))
                    {
                        prevNode = prevNode.NextChars[lowerPlayerTypedText[searchEndIndex]];
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
                        segments.Add(new ItemNameSegment(playerTypedText.Substring(searchStartIndex, searchEndIndex - searchStartIndex), lastNode.ItemName, true, wasPrecedingSpace));
                        break;
                    }
                }

                if (nodesTraversed.Count == 0)
                {
                    // current text is not an item name

                    if (playerTypedText[searchStartIndex] == ' ')
                    {
                        // break on spaces and create a new segment
                        segments.Add(new TextSegment(currentSegmentText, wasPrecedingSpace));
                        currentSegmentText = "";
                        wasPrecedingSpace = true;
                    }
                    else
                        currentSegmentText += playerTypedText[searchStartIndex];
                }
                else
                {
                    // current text is an item name

                    wasPrecedingSpace = false;
                }

                searchStartIndex += nodesTraversed.Count + 1;
            }

            segments.Add(new TextSegment(currentSegmentText, wasPrecedingSpace));

            return segments;
        }


        // private helper classes

        private class Node
        {
            public string ItemName { get; set; } = null;
            public Dictionary<char, Node> NextChars { get; } = new Dictionary<char, Node>();
        }

        private class ParseResult
        {
            public ChatLine ChatLine { get; set; }
            public List<TextSegment> Segments { get; set; }
        }
    }
}
