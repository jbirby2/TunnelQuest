using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;
using System.Threading;
using TunnelQuest.AppLogic.ChatSegments;
using System.Runtime.Caching;

namespace TunnelQuest.AppLogic
{
    public class ChatLogic
    {
        public const string AUCTION_TOKEN = "#TQAUC_";

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
                    var recreatedAuctions = new Dictionary<string, Auction>();
                    foreach (var segment in parsedLine.Segments)
                    {
                        if (segment is AuctionLinkSegment)
                        {
                            var auctionSegment = (AuctionLinkSegment)segment;
                            if (!recreatedAuctions.ContainsKey(auctionSegment.Auction.ItemName))
                                recreatedAuctions.Add(auctionSegment.Auction.ItemName, new Auction(auctionSegment.Auction, newChatLine.SentAt));
                            auctionSegment.Auction = recreatedAuctions[auctionSegment.Auction.ItemName];
                        }
                    }
                }

                // apply AuctionLogic
                var auctionLogic = new AuctionLogic(context);
                var normalizedAuctions = auctionLogic.GetNormalizedAuctions(serverCode, playerName, newChatLine.SentAt, parsedLine.GetAuctions());

                // go back through the chat segments and make them all point at the correct Auction objects
                foreach (var chatSegment in parsedLine.Segments)
                {
                    if (chatSegment is AuctionLinkSegment)
                    {
                        var auctionSegment = (AuctionLinkSegment)chatSegment;
                        auctionSegment.Auction = normalizedAuctions[auctionSegment.Auction.ItemName];
                    }
                }

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
