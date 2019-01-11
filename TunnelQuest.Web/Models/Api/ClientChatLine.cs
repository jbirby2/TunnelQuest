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
        public DateTime SentAt { get; set; }
        public Dictionary<long, ClientAuction> Auctions { get; set; }

        public ClientChatLine()
        {
        }

        public ClientChatLine (ChatLine line)
        {
            this.Id = line.ChatLineId;
            this.PlayerName = line.PlayerName;
            this.Text = line.Text;
            this.SentAt = line.SentAt;

            this.Auctions = new Dictionary<long, ClientAuction>();
            foreach (var auction in line.Auctions)
            {
                this.Auctions[auction.AuctionId] = new ClientAuction(auction.Auction);
            }
        }
    }
}
