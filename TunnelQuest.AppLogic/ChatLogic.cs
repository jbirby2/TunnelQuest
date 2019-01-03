using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;

namespace TunnelQuest.AppLogic
{
    public class ChatLogic
    {
        // static stuff

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

        public ChatLine ProcessLogLine(string serverCode, string logLine)
        {
            if (String.IsNullOrWhiteSpace(logLine))
                throw new Exception("logLine cannot be empty");

            serverCode = ServerCodes.All.Where(cleanCode => cleanCode.Equals(serverCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(serverCode))
                throw new Exception("serverCode cannot be empty");

            
            var newChatLine = new ChatLine();
            
            Monitor.Enter(CHAT_LINE_LOCK);
            try
            {
                newChatLine.ServerCode = serverCode;

                var parsedLine = parseChatLine(logLine);
                newChatLine.PlayerName = parsedLine.PlayerName;
                newChatLine.SentAt = DateTime.UtcNow;  // completely ignore the timestamp in the beginning of logLine, because the client device's internal clock could be wrong

                // search for existing auctions to reuse instead, based on AuctionLogic
                var auctionLogic = new AuctionLogic(context);
                auctionLogic.ApplyNewAuctionRules(serverCode, parsedLine);
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
            string[] lineWords = logLine.Split(' ', StringSplitOptions.None);

            if (lineWords.Length < 8 || lineWords[0][0] != '[' || lineWords[4][4] != ']' || lineWords[6] != "auctions,")
                throw new InvalidLogLineException(logLine);

            string playerName = lineWords[5];
            string playerTypedText = String.Join(' ', lineWords, 7, lineWords.Length - 7).Trim('\'');

            var parsedItemNames = parseItemNames(playerTypedText);

            // build wordList
            var wordList = new List<ChatWord>();
            string[] lineSplit = parsedItemNames.TokenizedText.Split(' ', StringSplitOptions.None);
            foreach (string wordText in lineSplit)
            {
                if (wordText.StartsWith(AuctionWord.AUCTION_TOKEN))
                {
                    int itemNameIndex = Convert.ToInt32(wordText.Substring(AuctionWord.AUCTION_TOKEN.Length));
                    wordList.Add(new ItemNameWord(parsedItemNames.ItemNames[itemNameIndex], itemNameIndex));
                }
                // STUB TO DO: more word types here (wtb/wts word, price word, etc)
                else
                    wordList.Add(new ChatWord(wordText));
            }


            // STUB TO DO - step through tokens and artificially add entries to itemNames for text that doesn't match known items.
            // Do it here, last, after creating all other strongly typed Words, because we can use them for hints to deduce weak
            // item names (i.e. anything between two PriceWords basically)


            // build auctions
            var auctions = new Dictionary<string, Auction>();
            for (int i = 0; i < wordList.Count; i++)
            {
                if (wordList[i] is ItemNameWord)
                {
                    string itemName = wordList[i].Text;
                    // if we find the same item name more than once, ignore it and only create an auction for the first occurence
                    if (auctions.ContainsKey(itemName))
                    {
                        wordList[i] = new AuctionWord(auctions[itemName]);
                       
                    }
                    else
                    {
                        // STUB TO DO: build properties below by examining previous and subsequent Words
                        DateTime createdAt = DateTime.UtcNow;
                        var newAuction = new Auction()
                        {
                            ItemName = itemName,
                            IsBuying = false,           // STUB
                            Price = 124,                // STUB
                            IsPriceNegotiable = false,  // STUB
                            IsAcceptingTrades = false,   // STUB
                            CreatedAt = createdAt,
                            UpdatedAt = createdAt
                        };

                        auctions.Add(itemName, newAuction);
                        wordList[i] = new AuctionWord(newAuction);
                    }
                }
            }

            return new ParsedChatLine(playerName, wordList.ToArray());
        }

        private ParseItemNamesResult parseItemNames(string text)
        {
            var result = new ParseItemNamesResult();
            result.ItemNames = new List<string>();
            result.TokenizedText = "";

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
                int? foundNameIndex = null;
                while (nodesTraversed.Count > 0)
                {
                    var lastNode = nodesTraversed.Pop();
                    if (lastNode.ItemName != null)
                    {
                        int existingItemNameIndex = result.ItemNames.IndexOf(lastNode.ItemName);

                        if (existingItemNameIndex < 0)
                        {
                            result.ItemNames.Add(lastNode.ItemName);
                            foundNameIndex = result.ItemNames.Count - 1;
                        }
                        else
                        {
                            foundNameIndex = existingItemNameIndex;
                        }
                        break;
                    }
                }

                int oldSearchStartIndex = searchStartIndex;
                searchStartIndex += nodesTraversed.Count + 1;

                if (foundNameIndex != null)
                    result.TokenizedText += AuctionWord.AUCTION_TOKEN + foundNameIndex.ToString();
                else
                    result.TokenizedText += text.Substring(oldSearchStartIndex, searchStartIndex - oldSearchStartIndex);
            }

            return result;
        }


        // private helper classes

        private class Node
        {
            public string ItemName { get; set; } = null;
            public Dictionary<char, Node> NextChars { get; } = new Dictionary<char, Node>();
        }

        private class ParseItemNamesResult
        {
            public string TokenizedText { get; set; }
            public List<string> ItemNames { get; set; }
        }
    }
}
