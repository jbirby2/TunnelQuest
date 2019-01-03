using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TunnelQuest.Data.Models;

namespace TunnelQuest.AppLogic
{
    internal class ParsedChatLine
    {
        public string PlayerName { get; private set; }
        public ChatWord[] Words { get; private set; }

        public ParsedChatLine(string playerName, ChatWord[] words)
        {
            this.PlayerName = playerName;
            this.Words = words;
        }

        public IEnumerable<Auction> GetAuctions()
        {
            var auctions = new List<Auction>();
            foreach (ChatWord word in this.Words)
            {
                if (word is AuctionWord)
                {
                    var auction = ((AuctionWord)word).Auction;
                    if (!auctions.Contains(auction))
                        auctions.Add(auction);
                }
            }
            return auctions;
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < this.Words.Length; i++)
            {
                if (i > 0)
                    str += " ";
                str += this.Words[i].ToString();
            }
            return str;
        }
    }
}
