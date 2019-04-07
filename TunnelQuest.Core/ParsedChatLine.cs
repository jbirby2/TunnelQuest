using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TunnelQuest.Core.Models;
using TunnelQuest.Core.ChatSegments;
using TunnelQuest.Core;

namespace TunnelQuest.Core
{
    internal class ParsedChatLine
    {
        // static

        private static Node itemNamesRootNode = null;
        
        // static constructor
        static ParsedChatLine()
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


        // non-static

        public string PlayerTypedText { get; private set; }
        public List<TextSegment> Segments { get; private set; }
        public Dictionary<string, Auction> Auctions { get; private set; }
        public List<ChatLineToken> Tokens { get; private set; }


        public ParsedChatLine(string playerTypedText, TunnelQuestContext context, DateTime timestamp)
        {
            this.Segments = new List<TextSegment>();
            this.Auctions = new Dictionary<string, Auction>();
            this.Tokens = new List<ChatLineToken>();

            this.PlayerTypedText = playerTypedText;

            // At this point, Segments is an empty list.  The next function will parse PlayerTypedText
            // and fill Segments with a combination of TextSegments and ItemNameSegments.

            parseItemNames(timestamp);

            // At this point, the only types of segments in Segments are TextSegments and ItemNameSegments.  We want
            // to loop through and attempt to replace each of the generic TextSegments (which represent unrecognized text)
            // with more specific Segments which represent recognized data elements.

            for (int i = 0; i < this.Segments.Count; i++)
            {
                var segment = this.Segments[i];
                if (segment.GetType() == typeof(TextSegment))
                {
                    if (segment.Text.Contains(ChatLogic.CHAT_TOKEN))
                    {
                        // in case anybody tries to be mischevious and actually type the token string into chat
                        this.Segments[i] = new TextSegment(this, "clever girl", segment.HasPrecedingSpace);
                    }
                    else
                    {
                        TextSegment parsedSegment = PriceSegment.TryParse(this, segment);

                        if (parsedSegment == null)
                            parsedSegment = SeparatorSegment.TryParse(this, segment);

                        if (parsedSegment == null)
                            parsedSegment = BuySellTradeSegment.TryParse(this, segment);

                        if (parsedSegment == null)
                            parsedSegment = OrBestOfferSegment.TryParse(this, segment);

                        if (parsedSegment != null)
                            this.Segments[i] = parsedSegment;
                    }
                }
            }

            // Now that we've parsed all the segments we can recognize, go back through  and see if we 
            // can intuit any non-linked auctions (e.g. "WTS jboots mq 5k")

            for (int i = 0; i < Segments.Count; i++)
            {
                if (Segments[i].GetType() == typeof(TextSegment))
                {
                    // First, merge this TextSegment with any other TextSegments that come immediately after it
                    var indexesToMerge = new List<int>();
                    indexesToMerge.Add(i);
                    string mergedText = Segments[i].Text;
                    bool mergedTextHasPrecedingSpace = Segments[i].HasPrecedingSpace;
                    for (int j = i + 1; j < Segments.Count; j++)
                    {
                        if (Segments[j].GetType() == typeof(TextSegment))
                        {
                            indexesToMerge.Add(j);

                            if (Segments[j].HasPrecedingSpace)
                                mergedText += ' ';
                            mergedText += Segments[j].Text;
                        }
                        else
                            break;
                    }
                    indexesToMerge.Reverse(); // delete indexes from back-to-front so that each deleted index doesn't shift the remaining ones into new positions
                    foreach (int index in indexesToMerge)
                    {
                        Segments.RemoveAt(index);
                    }

                    // Now that we've got a string containing the text of all the adjacent TextSegments, decide
                    // whether it's something we should create an auction for
                    bool createAuction;

                    // Assume that any unrecognized text which comes *immediately* before or after after an item link is a 
                    // description of the item, and *not* something we should create an auction for.
                    if (i > 0 && Segments[i - 1] is ItemNameSegment)
                        createAuction = false;
                    else if (i < Segments.Count && Segments[i] is ItemNameSegment)
                        createAuction = false;
                    else if (mergedText.StartsWith("PST", StringComparison.InvariantCultureIgnoreCase))
                        createAuction = false;
                    else
                        createAuction = true;

                    // STUB TODO - will probably end up adding more fine-tuned logic here after testing more
                    // real world auction logs

                    if (createAuction)
                        Segments.Insert(i, new ItemNameSegment(this, mergedText, mergedText.Trim(), false, mergedTextHasPrecedingSpace)); // the .Trim() is important
                    else
                        Segments.Insert(i, new TextSegment(this, mergedText, mergedTextHasPrecedingSpace));
                }
            }

            // Now that we've created all of the ItemNameSegments, loop through all the segments one last time and
            // use their values to build the Auction objects.  If the same item name is found more than once, only create
            // one single auction for the item.

            BuySellTradeSegment lastFoundBuySellTrade = null;
            var itemsSinceLastSeparator = new List<ItemNameSegment>();

            // used by PriceSegment to build its chat token
            var itemSegmentIndexes = new Dictionary<ItemNameSegment, int>();
            int lastItemNameIndex = -1;

            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];

                if (segment is ItemNameSegment)
                {
                    var itemNameSegment = (ItemNameSegment)segment;

                    lastItemNameIndex++;
                    itemSegmentIndexes.Add(itemNameSegment, lastItemNameIndex);
                    itemsSinceLastSeparator.Add(itemNameSegment);

                    itemNameSegment.IsTokenized = true;
                    var itemToken = new ChatLineToken()
                    {
                        TokenTypeCode = ChatLineTokenTypeCodes.Item,
                        Index = Convert.ToByte(this.Tokens.Count)
                    };
                    itemToken.Properties.Add(new ChatLineTokenProperty()
                    {
                        ChatLineTokenId = itemToken.ChatLineTokenId,
                        ChatLineToken = itemToken,
                        Property = "text",
                        Value = itemNameSegment.Text
                    });
                    itemToken.Properties.Add(new ChatLineTokenProperty()
                    {
                        ChatLineTokenId = itemToken.ChatLineTokenId,
                        ChatLineToken = itemToken,
                        Property = "isKnown",
                        Value = itemNameSegment.IsKnownItem ? "1" : "0"
                    });
                    itemToken.Properties.Add(new ChatLineTokenProperty()
                    {
                        ChatLineTokenId = itemToken.ChatLineTokenId,
                        ChatLineToken = itemToken,
                        Property = "itemName",
                        Value = itemNameSegment.ItemName
                    });
                    this.Tokens.Add(itemToken);

                    Auction auction;
                    if (!this.Auctions.ContainsKey(itemNameSegment.ItemName))
                    {
                        auction = new Auction()
                        {
                            ItemName = itemNameSegment.ItemName,
                            IsKnownItem = itemNameSegment.IsKnownItem,
                            CreatedAt = timestamp,
                            UpdatedAt = timestamp
                        };

                        this.Auctions.Add(itemNameSegment.ItemName, auction);
                    }
                    else
                    {
                        auction = this.Auctions[itemNameSegment.ItemName];
                    }

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
                    bool isBuying = false;
                    foreach (var itemNameSegment in itemsSinceLastSeparator)
                    {
                        var auction = this.Auctions[itemNameSegment.ItemName];

                        if (auction.Price == null)
                        {
                            auction.Price = priceSegment.Price;

                            priceSegment.IsTokenized = true;
                            itemSegmentIndexesWithThisPrice.Add(itemSegmentIndexes[itemNameSegment]);
                            isBuying = auction.IsBuying;
                        }
                    }

                    if (priceSegment.IsTokenized)
                    {
                        var priceToken = new ChatLineToken()
                        {
                            TokenTypeCode = ChatLineTokenTypeCodes.Price,
                            Index = Convert.ToByte(this.Tokens.Count)
                        };

                        priceToken.Properties.Add(new ChatLineTokenProperty()
                        {
                            ChatLineTokenId = priceToken.ChatLineTokenId,
                            ChatLineToken = priceToken,
                            Property = "isBuying",
                            Value = isBuying ? "1" : "0"
                        });

                        priceToken.Properties.Add(new ChatLineTokenProperty()
                        {
                            ChatLineTokenId = priceToken.ChatLineTokenId,
                            ChatLineToken = priceToken,
                            Property = "price",
                            Value = priceSegment.Price.ToString()
                        });

                        priceToken.Properties.Add(new ChatLineTokenProperty()
                        {
                            ChatLineTokenId = priceToken.ChatLineTokenId,
                            ChatLineToken = priceToken,
                            Property = "items",
                            Value = String.Join(',', itemSegmentIndexesWithThisPrice)
                        });

                        priceToken.Properties.Add(new ChatLineTokenProperty()
                        {
                            ChatLineTokenId = priceToken.ChatLineTokenId,
                            ChatLineToken = priceToken,
                            Property = "text",
                            Value = priceSegment.Text
                        });

                        this.Tokens.Add(priceToken);
                    }
                }
                else if (segment is OrBestOfferSegment)
                {
                    foreach (var itemNameSegment in itemsSinceLastSeparator)
                    {
                        var auction = this.Auctions[itemNameSegment.ItemName];

                        if (auction.Price != null)
                            auction.IsOrBestOffer = true;
                    }
                }
                else if (segment is SeparatorSegment)
                {
                    itemsSinceLastSeparator.Clear();
                }
            }
        }

        public override string ToString()
        {
            string str = "";

            foreach (var segment in this.Segments)
            {
                if (segment.HasPrecedingSpace)
                    str += ' ';

                if (segment.IsTokenized)
                    str += ChatLogic.CHAT_TOKEN;
                else
                    str += segment.Text;
            }
            return str;
        }

        public TextSegment NextSegment(TextSegment segment)
        {
            if (!Segments.Contains(segment))
                throw new Exception("this.Segments does not contain parameter segment");

            int index = this.Segments.IndexOf(segment);
            if (index + 1 < this.Segments.Count)
                return this.Segments[index + 1];
            else
                return null;
        }

        public TextSegment PrevSegment(TextSegment segment)
        {
            if (!Segments.Contains(segment))
                throw new Exception("this.Segments does not contain parameter segment");

            int index = this.Segments.IndexOf(segment);
            if (index - 1 > 0)
                return this.Segments[index - 1];
            else
                return null;
        }


        // private

        private void parseItemNames(DateTime timestamp)
        {
            string lowerPlayerTypedText = PlayerTypedText.ToLower();

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
                        Segments.Add(new ItemNameSegment(this, PlayerTypedText.Substring(searchStartIndex, searchEndIndex - searchStartIndex), lastNode.ItemName, true, wasPrecedingSpace));
                        break;
                    }
                }

                if (nodesTraversed.Count == 0)
                {
                    // current text is not an item name

                    if (PlayerTypedText[searchStartIndex] == ' ')
                    {
                        // break on spaces and create a new segment
                        Segments.Add(new TextSegment(this, currentSegmentText, wasPrecedingSpace));
                        currentSegmentText = "";
                        wasPrecedingSpace = true;
                    }
                    else
                        currentSegmentText += PlayerTypedText[searchStartIndex];
                }
                else
                {
                    // current text is an item name

                    wasPrecedingSpace = false;
                }

                searchStartIndex += nodesTraversed.Count + 1;
            }

            Segments.Add(new TextSegment(this, currentSegmentText, wasPrecedingSpace));
        }


        // private helper classes

        private class Node
        {
            public string ItemName { get; set; } = null;
            public Dictionary<char, Node> NextChars { get; } = new Dictionary<char, Node>();
        }
    }
}
