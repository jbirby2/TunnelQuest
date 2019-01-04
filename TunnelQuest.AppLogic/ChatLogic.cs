using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;
using TunnelQuest.AppLogic.ChatSegments;

namespace TunnelQuest.AppLogic
{
    public class ChatLogic
    {
        // static stuff

        private static readonly Dictionary<string, short> tokenCache = new Dictionary<string, short>();
        private static readonly object CHAT_LINE_LOCK = new object();
        private static Node rootNode = new Node();

        // static constructor pulls a list of every item name and builds a big tree of letter paths for every possible name
        static ChatLogic()
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
        public ChatLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }

        public ChatLine ProcessLogLine(string authTokenValue, string serverCode, string logLine)
        {
            if (String.IsNullOrWhiteSpace(authTokenValue))
                throw new Exception("authTokenValue cannot be empty");

            if (String.IsNullOrWhiteSpace(logLine))
                throw new Exception("logLine cannot be empty");

            serverCode = ServerCodes.All.Where(cleanCode => cleanCode.Equals(serverCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(serverCode))
                throw new Exception("serverCode cannot be empty");

            if (!tokenCache.ContainsKey(authTokenValue))
            {
                short tokenId = (from token in context.AuthTokens
                                 where token.Value == authTokenValue
                                 select token.AuthTokenId).FirstOrDefault();

                if (tokenId <= 0)
                    throw new InvalidAuthTokenException();
                else
                    tokenCache[authTokenValue] = tokenId;
            }

            var newChatLine = new ChatLine();
            newChatLine.AuthTokenId = tokenCache[authTokenValue];

            Monitor.Enter(CHAT_LINE_LOCK);
            try
            {
                newChatLine.ServerCode = serverCode;

                var parsedLine = parseChatLine(logLine);
                newChatLine.PlayerName = parsedLine.PlayerName;
                newChatLine.SentAt = DateTime.UtcNow;  // completely ignore the timestamp in the beginning of logLine, because the client device's internal clock could be wrong

                // search for existing auctions to reuse instead, based on AuctionLogic
                var auctionLogic = new AuctionLogic(context);
                auctionLogic.ApplyAuctionRules(serverCode, parsedLine);
                var finalAuctions = parsedLine.GetAuctions();
                foreach (var auction in finalAuctions)
                {
                    newChatLine.Auctions.Add(new ChatLineAuction()
                    {
                        ChatLine = newChatLine,
                        Auction = auction
                    });
                }
                
                // save everything to database
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.AddRange(finalAuctions.Where(auction => auction.AuctionId == 0));
                        context.SaveChanges();

                        // Now that we've saved the auctions to the database and populated the Auction objects
                        // with their real auction_id's, we can finally set newChatLine.Text
                        newChatLine.Text = parsedLine.ToString();
                        context.Add(newChatLine);
                        context.SaveChanges();

                        transaction.Commit();
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
            
            return newChatLine;
        }


        // private

        private ParsedChatLine parseChatLine(string logLine)
        {
            string[] lineSegments = logLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (lineSegments.Length < 8 || lineSegments[0][0] != '[' || lineSegments[4][4] != ']' || lineSegments[6] != "auctions,")
                throw new InvalidLogLineException(logLine);

            string playerName = lineSegments[5];
            string playerTypedText = String.Join(' ', lineSegments, 7, lineSegments.Length - 7).Trim('\'');

            var segmentList = parseItemNames(playerTypedText);

            // At this point, the only types of segments in segmentList are AuctionSegments and ChatSegments.  We want
            // to loop through and attempt to replace each of the generic ChatSegments (which represent unknown text)
            // with more specific Segments that represent data elements.
            for (int i = 0; i < segmentList.Count; i++)
            {
                var segment = segmentList[i];
                if ((segment is AuctionSegment) == false)
                {
                    ChatSegment parsedSegment = NumberSegment.TryParse(segment.Text);

                    // STUB TO DO: more segment types here (wtb/wts segment, price segment, etc)

                    if (parsedSegment != null)
                        segmentList[i] = parsedSegment;
                }
            }


            // STUB TO DO: build properties below for each AuctionSegment by examining previous and subsequent Segments
            /*
             * IsBuying = false,           // STUB
                Price = 124,                // STUB
                IsPriceNegotiable = false,  // STUB
                IsAcceptingTrades = false,   // STUB
            */


            // STUB TO DO - step through tokens and artificially add entries to itemNames for text that doesn't match known items.
            // Do it here, last, after creating all other strongly typed segments, because we can use them for hints to deduce weak
            // item names (i.e. anything between two NumberSegments basically)


            return new ParsedChatLine(playerName, segmentList);
        }

        private List<ChatSegment> parseItemNames(string playerTypedText)
        {
            var segmentList = new List<ChatSegment>();

            int searchStartIndex = 0;
            string currentSegmentText = "";
            while (searchStartIndex < playerTypedText.Length)
            {
                Node prevNode = rootNode;
                var nodesTraversed = new Stack<Node>();
                int searchEndIndex = searchStartIndex;

                // build nodesTraversed
                while (searchEndIndex < playerTypedText.Length)
                {
                    if (prevNode.NextChars.ContainsKey(playerTypedText[searchEndIndex]))
                    {
                        prevNode = prevNode.NextChars[playerTypedText[searchEndIndex]];
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
                        segmentList.Add(new AuctionSegment(new Auction()
                        {
                            ItemName = lastNode.ItemName
                        }));

                        break;
                    }
                }

                if (nodesTraversed.Count == 0)
                {
                    if (playerTypedText[searchStartIndex] == ' ')
                    {
                        segmentList.Add(new ChatSegment(currentSegmentText));
                        currentSegmentText = "";
                    }
                    else
                        currentSegmentText += playerTypedText[searchStartIndex];
                }

                searchStartIndex += nodesTraversed.Count + 1;
            }

            return segmentList;
        }


        // private helper classes

        private class Node
        {
            public string ItemName { get; set; } = null;
            public Dictionary<char, Node> NextChars { get; } = new Dictionary<char, Node>();
        }

    }
}
