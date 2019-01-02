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
            newChatLine.ServerCode = serverCode;

            Monitor.Enter(CHAT_LINE_LOCK);
            try
            {
                string[] lineWords = logLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (lineWords.Length < 8 || lineWords[0][0] != '[' || lineWords[4][4] != ']' || lineWords[6] != "auctions,")
                    throw new InvalidLogLineException(logLine);

                string logLineWithoutTimestamp = logLine.Substring(27);
                
                newChatLine.Text = logLineWithoutTimestamp;
                newChatLine.PlayerName = lineWords[5];
                newChatLine.SentAt = DateTime.UtcNow;  // completely ignore the timestamp in the beginning of logLine, because the client device's internal clock could be wrong
                
                // attach the new and/or existing auctions for this chat line
                var auctionLogic = new AuctionLogic(context);
                string playerTypedText = String.Join(' ', lineWords, 7, lineWords.Length - 7).Trim('\'');
                var auctions = auctionLogic.GetAuctions(newChatLine.ServerCode, newChatLine.PlayerName, playerTypedText);
                foreach (var auction in auctions)
                {
                    newChatLine.Auctions.Add(new ChatLineAuction()
                    {
                        ChatLine = newChatLine,
                        ChatLineId = newChatLine.ChatLineId,
                        Auction = auction,
                        AuctionId = auction.AuctionId
                    });
                }

                context.Add(newChatLine);
                context.SaveChanges();
            }
            finally
            {
                Monitor.Exit(CHAT_LINE_LOCK);
            }
            
            return newChatLine;
        }


    }
}
