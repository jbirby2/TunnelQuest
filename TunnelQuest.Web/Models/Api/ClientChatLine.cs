using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientChatLine
    {
        public long Id { get; set; }
        public string PlayerName { get; set; }
        public string Text { get; set; }
        public Dictionary<long, ClientAuction> Auctions { get; set; }
        public DateTime SentAtString { get; set; } // named String even though it's a DateTime in C# because it will be serialized as a string in javascript

        public ClientChatLine()
        {
        }

        public ClientChatLine (ChatLine line, HashSet<long> filteredAuctionIds)
        {
            this.Id = line.ChatLineId;
            this.PlayerName = line.PlayerName;
            this.Text = line.Text;
            this.SentAtString = line.SentAt;

            this.Auctions = new Dictionary<long, ClientAuction>();
            foreach (var auction in line.Auctions)
            {
                this.Auctions[auction.AuctionId] = new ClientAuction(auction, filteredAuctionIds == null ? false : filteredAuctionIds.Contains(auction.AuctionId));
            }
        }
    }
}
