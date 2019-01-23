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
        public Dictionary<long, ClientChatLineAuctionInfo> Auctions{ get; set; }

        public ClientChatLine()
        {
        }

        public ClientChatLine (ChatLine line)
        {
            this.Id = line.ChatLineId;
            this.PlayerName = line.PlayerName;
            this.Text = line.Text;
            this.SentAtString = line.SentAt;

            this.Auctions = new Dictionary<long, ClientChatLineAuctionInfo>();
            foreach (var lineAuction in line.Auctions)
            {
                this.Auctions[lineAuction.AuctionId] = new ClientChatLineAuctionInfo()
                {
                    IsKnownItem = lineAuction.Auction.IsKnownItem,
                    ItemName = lineAuction.Auction.ItemName
                };
            }
        }

        
        // inner class
        public class ClientChatLineAuctionInfo
        {
            public bool IsKnownItem { get; set; }
            public string ItemName { get; set; }
        }
    }
}
