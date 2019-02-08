﻿using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;
using TunnelQuest.AppLogic.ChatSegments;
using System.Runtime.Caching;
using Microsoft.EntityFrameworkCore;

namespace TunnelQuest.AppLogic
{
    public class ChatLogic
    {
        public const string ITEM_NAME_TOKEN = "#TQITEM#";
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

        public ChatLine[] GetLines(string serverCode, long? minId = null, long? maxId = null, int? maxResults = null)
        {
            if (maxResults == null)
                maxResults = MAX_CHAT_LINES;

            var query = context.ChatLines
                .Include(line => line.Auctions)
                    .ThenInclude(chatLineAuctions => chatLineAuctions.Auction)
                .Where(line => line.ServerCode == serverCode);

            if (minId != null)
                query = query.Where(line => line.ChatLineId >= minId.Value);

            if (maxId != null)
                query = query.Where(line => line.ChatLineId <= maxId.Value);

            return query
                .OrderByDescending(line => line.ChatLineId) // order by descending in the sql query, to make sure we get the most recent lines if we hit the limit imposed by maxResults
                .Take(maxResults.Value)
                .ToArray() // call .ToArray() to force entity framework to execute the query and get the results from the database
                .OrderBy(line => line.ChatLineId) // now that we've got the results from the database (possibly truncated by maxResults), re-order them correctly
                .ToArray();
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

            string[] lineWords = logLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
                newChatLine.SentAt = DateTime.UtcNow;  // completely ignore the timestamp in the beginning of logLine, because the client device's internal clock could be wrong

                ParsedChatLine parsedLine;
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

                    // create new Auction objects instead of reusing the Auction objects that were previously saved to database,
                    // just to be sure there's no unexpected entity framework behavior
                    foreach (string itemName in parsedLine.Auctions.Keys.ToArray())
                    {
                        parsedLine.Auctions[itemName] = new Auction(parsedLine.Auctions[itemName], newChatLine.SentAt);
                    }
                }

                // apply AuctionLogic
                var auctionLogic = new AuctionLogic(context);
                var normalizedAuctions = auctionLogic.GetNormalizedAuctions(serverCode, playerName, newChatLine.SentAt, parsedLine.Auctions);

                // attach the Auction objects to the ChatLine object
                foreach (var auction in normalizedAuctions.Values)
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
                        context.AddRange(normalizedAuctions.Values.Where(auction => auction.AuctionId <= 0));
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

                MemoryCache.Default.Set(cacheKey, new CachedLine()
                {
                    AuthTokenId = authTokenId,
                    PlayerName = playerName,
                    ServerCode = serverCode,
                    Timestamp = newChatLine.SentAt,
                    ParsedLine = parsedLine
                }, DateTimeOffset.Now.AddMinutes(10));

                return newChatLine;
            }
            finally
            {
                Monitor.Exit(CHAT_LINE_LOCK);
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
