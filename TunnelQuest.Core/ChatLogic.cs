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
        
        
        // non-static stuff

        private TunnelQuestContext context;
        public ChatLogic(TunnelQuestContext _context)
        {
            if (_context == null)
                throw new Exception("_context cannot be null");

            this.context = _context;
        }

        public ChatLine[] GetLines(ChatLinesQuery criteria)
        {
            var query = context.ChatLines
                .Include(chatLine => chatLine.Tokens)
                    .ThenInclude(chatLineToken => chatLineToken.Properties)
                .Where(line => line.ServerCode == criteria.ServerCode);

            if (criteria.MinimumId != null)
                query = query.Where(line => line.ChatLineId >= criteria.MinimumId.Value);

            if (criteria.MaximumId != null)
                query = query.Where(line => line.ChatLineId <= criteria.MaximumId.Value);

            // order by descending in the sql query, to make sure we get the most recent lines if we hit the limit imposed by maxResults
            query = query.OrderByDescending(line => line.ChatLineId);

            if (criteria.MaxResults != null)
                query = query.Take(criteria.MaxResults.Value);

            return query.ToArray() // call .ToArray() to force entity framework to execute the query and get the results from the database
                .OrderBy(line => line.ChatLineId) // now that we've got the results from the database (possibly truncated by maxResults), re-order them correctly
                .ToArray();
        }

        public ProcessLogLineResult ProcessLogLine(string authTokenValue, string serverCode, string logLine, DateTime timeStamp)
        {
            if (String.IsNullOrWhiteSpace(authTokenValue))
                throw new Exception("authTokenValue cannot be empty");

            if (String.IsNullOrWhiteSpace(logLine))
                throw new Exception("logLine cannot be empty");

            serverCode = ServerCodes.All.Where(cleanCode => cleanCode.Equals(serverCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(serverCode))
                throw new Exception("serverCode cannot be empty");

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

            string playerName = lineWords[0];
            string playerTypedText = String.Join(' ', lineWords, 2, lineWords.Length - 2).Trim('\'');

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

                var newChatLine = new ChatLine();
                newChatLine.AuthTokenId = tokenCache[authTokenValue];
                newChatLine.ServerCode = serverCode;
                newChatLine.PlayerName = playerName;
                newChatLine.SentAt = timeStamp;

                ParsedChatLine parsedLine;

                /* STUB disable incoming line caching for now
                string cacheKey = serverCode + logLine;
                var cachedLine = (CachedLine)MemoryCache.Default[cacheKey];
                if (cachedLine == null)
                {
                    parsedLine = new ParsedChatLine(playerTypedText, context, newChatLine.SentAt);
                }
                else
                {
                    // Kill two birds with one stone:
                    //  1. prevent the same line being posted by multiple TunnelWatchers
                    //  2. prevent excessive spam of duplicate lines (probably not really necessary but 1. definitely is)
                    if ((DateTime.UtcNow - cachedLine.Timestamp) < DUPLICATE_LINE_FILTER_THRESHOLD)
                        return null;

                    parsedLine = cachedLine.ParsedLine;

                    // create new database objects instead of reusing the objects that were previously saved to database,
                    // just to be sure there's no unexpected entity framework behavior
                    foreach (string itemName in parsedLine.Auctions.Keys.ToArray())
                    {
                        parsedLine.Auctions[itemName] = new Auction(parsedLine.Auctions[itemName], newChatLine.SentAt);
                    }
                    for (int i = 0; i < parsedLine.Tokens.Count; i++)
                    {
                        parsedLine.Tokens[i] = new ChatLineToken(parsedLine.Tokens[i]);
                    }
                }
                */
                parsedLine = new ParsedChatLine(playerTypedText, context, newChatLine.SentAt); // PART OF THE STUB
                // END DISABLEED CACHING STUB

                // attach the parsed tokens to the ChatLine object
                foreach (var token in parsedLine.Tokens)
                {
                    token.ChatLineId = newChatLine.ChatLineId;
                    token.ChatLine = newChatLine;
                    newChatLine.Tokens.Add(token);
                }

                // apply AuctionLogic
                var auctionLogic = new AuctionLogic(context);
                var normalizedAuctions = auctionLogic.GetNormalizedAuctions(serverCode, playerName, newChatLine.SentAt, parsedLine.Auctions);

                // fill in some additional auction data properties
                foreach (var auction in normalizedAuctions.Values)
                {
                    auction.MostRecentChatLine = newChatLine;
                    auction.MostRecentChatLineId = newChatLine.ChatLineId;
                    
                    if (auction.AuctionId <= 0)
                    {
                        auction.PlayerName = newChatLine.PlayerName;
                        auction.ServerCode = newChatLine.ServerCode;
                    }
                }

                // save everything to database
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        newChatLine.Text = parsedLine.ToString();
                        context.Add(newChatLine);
                        context.SaveChanges();

                        // now that we've saved the new ChatLine and it has its permanent ChatLineId, we can save the auctions
                        
                        // insert any NEW auction records (not pre-existing auction records)
                        context.AddRange(normalizedAuctions.Values.Where(auction => auction.AuctionId <= 0));

                        // also insert any new UnknownItems
                        foreach (var auction in normalizedAuctions.Values.Where(auction => auction.IsKnownItem == false))
                        {
                            if (context.UnknownItems.Count(unknownItem => unknownItem.ServerCode == serverCode && unknownItem.IsBuying == auction.IsBuying && unknownItem.ItemName == auction.ItemName) == 0)
                            {
                                context.UnknownItems.Add(new UnknownItem() {
                                    ServerCode = serverCode,
                                    IsBuying = auction.IsBuying,
                                    ItemName = auction.ItemName,
                                    CreatedAt = auction.CreatedAt
                                });
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                // STUB disabled caching STUB
                /*
                MemoryCache.Default.Set(cacheKey, new CachedLine()
                {
                    AuthTokenId = authTokenId,
                    PlayerName = playerName,
                    ServerCode = serverCode,
                    Timestamp = newChatLine.SentAt,
                    ParsedLine = parsedLine
                }, DateTimeOffset.Now.AddMinutes(10));
                */

                return new ProcessLogLineResult(newChatLine, normalizedAuctions.Values.ToArray());
            }
            finally
            {
                Monitor.Exit(CHAT_LINE_LOCK);
            }
        }


        // public helper class

        public class ProcessLogLineResult
        {
            public ChatLine NewLine { get; set; }
            public Auction[] NewAuctions { get; set; }

            public ProcessLogLineResult(ChatLine newLine, Auction[] newAuctions)
            {
                this.NewLine = newLine;
                this.NewAuctions = newAuctions;
            }
        }

        // private helper class

        private class CachedLine
        {
            public int AuthTokenId { get; set; }
            public string PlayerName { get; set; }
            public string ServerCode { get; set; }
            public DateTime Timestamp { get; set; }
            public ParsedChatLine ParsedLine { get; set; }
        }
    }
}
