using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientChatLine
    {
        public long Id { get; set; }
        public string PlayerName { get; set; }
        public string Text { get; set; }
        public DateTime SentAtString { get; set; } // named String even though it's a DateTime in C# because it will be serialized as a string in javascript
        public long[] AuctionIds { get; set; }

        public ClientChatLine()
        {
        }

        public ClientChatLine (ChatLine line)
        {
            this.Id = line.ChatLineId;
            this.PlayerName = line.PlayerName;
            this.Text = line.Text;
            this.SentAtString = line.SentAt;

            this.AuctionIds = new long[line.Auctions.Count];
            int i = 0;
            foreach (var lineAuction in line.Auctions)
            {
                this.AuctionIds[i] = lineAuction.Auction.AuctionId;
                i++;
            }
        }
    }
}
