using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientAuction
    {
        public long Id { get; set; }
        public string ItemName { get; set; }
        public bool IsKnownItem { get; set; }
        public bool IsBuying { get; set; }
        public int? Price { get; set; }
        public bool IsOrBestOffer { get; set; }
        public bool IsAcceptingTrades { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public ClientAuction()
        {
        }

        public ClientAuction(Auction auction)
        {
            this.Id = auction.AuctionId;
            this.ItemName = auction.ItemName;
            this.IsKnownItem = auction.IsKnownItem;
            this.IsBuying = auction.IsBuying;
            this.Price = auction.Price;
            this.IsOrBestOffer = auction.IsOrBestOffer;
            this.IsAcceptingTrades = auction.IsAcceptingTrades;
            this.CreatedAt = auction.CreatedAt;
            this.UpdatedAt = auction.UpdatedAt;
        }
    }
}
