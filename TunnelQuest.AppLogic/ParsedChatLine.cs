using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TunnelQuest.Data.Models;
using TunnelQuest.AppLogic.ChatSegments;

namespace TunnelQuest.AppLogic
{
    internal class ParsedChatLine
    {
        // static

        private static Node rootNode = null;

        private static void ensureStaticItemNamesTree(TunnelQuestContext context)
        {
            if (rootNode == null)
            {
                rootNode = new Node();

                IEnumerable<string> itemNames;

                itemNames = (from item in context.Items
                             orderby item.ItemName
                             select item.ItemName).ToArray();

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
        }

        // non-static

        public string PlayerTypedText { get; private set; }
        public List<TextSegment> Segments { get; private set; }
        public Dictionary<string, Auction> Auctions { get; private set; }


        public ParsedChatLine(string playerTypedText, TunnelQuestContext context, DateTime timestamp)
        {
            this.Segments = new List<TextSegment>();
            this.Auctions = new Dictionary<string, Auction>();

            this.PlayerTypedText = playerTypedText;

            ParsedChatLine.ensureStaticItemNamesTree(context);

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
                    if (segment.Text.Contains(ChatLogic.OUTER_CHAT_TOKEN) || segment.Text.Contains(ChatLogic.INNER_CHAT_TOKEN))
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
                        Segments.Insert(i, new ItemNameSegment(this, mergedText, false, mergedTextHasPrecedingSpace));
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

                    Auction auction;
                    if (!this.Auctions.ContainsKey(itemNameSegment.Text))
                    {
                        auction = new Auction()
                        {
                            ItemName = itemNameSegment.ItemName,
                            IsKnownItem = itemNameSegment.IsKnownItem,
                            CreatedAt = timestamp,
                            UpdatedAt = timestamp
                        };

                        this.Auctions.Add(itemNameSegment.Text, auction);
                    }
                    else
                    {
                        auction = this.Auctions[itemNameSegment.Text];
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

                    foreach (var itemNameSegment in itemsSinceLastSeparator)
                    {
                        var auction = this.Auctions[itemNameSegment.Text];

                        if (auction.Price == null)
                        {
                            auction.Price = priceSegment.Price;

                            // update the priceSegment too, for later when it builds its chat token
                            priceSegment.UsedByItemIndexes.Add(itemSegmentIndexes[itemNameSegment]);
                            priceSegment.IsBuying = auction.IsBuying;
                        }
                    }
                }
                else if (segment is OrBestOfferSegment)
                {
                    foreach (var itemNameSegment in itemsSinceLastSeparator)
                    {
                        var auction = this.Auctions[itemNameSegment.Text];

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
            int searchStartIndex = 0;
            string currentSegmentText = "";
            bool wasPrecedingSpace = false;
            while (searchStartIndex < PlayerTypedText.Length)
            {
                Node prevNode = rootNode;
                var nodesTraversed = new Stack<Node>();
                int searchEndIndex = searchStartIndex;

                // build nodesTraversed
                while (searchEndIndex < PlayerTypedText.Length)
                {
                    if (prevNode.NextChars.ContainsKey(PlayerTypedText[searchEndIndex]))
                    {
                        prevNode = prevNode.NextChars[PlayerTypedText[searchEndIndex]];
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
                        Segments.Add(new ItemNameSegment(this, lastNode.ItemName, true, wasPrecedingSpace));
                        break;
                    }
                }

                if (nodesTraversed.Count == 0)
                {
                    // no item was found

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
                    // item was found

                    wasPrecedingSpace = false;
                }

                searchStartIndex += nodesTraversed.Count + 1;
            }

            //if (currentSegmentText != "")
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
