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
        public long? PreviousAuctionId { get; set; }
        public string ItemName { get; set; }
        public bool IsKnownItem { get; set; }
        public bool IsBuying { get; set; }
        public int? Price { get; set; }
        public bool IsOrBestOffer { get; set; }
        public bool IsAcceptingTrades { get; set; }
        public DateTime CreatedAtString { get; set; } // named String even though it's a DateTime in C# because it will be serialized as a string in javascript
        public DateTime UpdatedAtString { get; set; } // named String even though it's a DateTime in C# because it will be serialized as a string in javascript
        public long ChatLineId { get; set; }
        
        public ClientAuction()
        {
        }

        public ClientAuction(Auction auction)
        {
            this.Id = auction.AuctionId;
            this.PreviousAuctionId = auction.PreviousAuctionId;
            this.ItemName = auction.ItemName;
            this.IsKnownItem = auction.IsKnownItem;
            this.IsBuying = auction.IsBuying;
            this.Price = auction.Price;
            this.IsOrBestOffer = auction.IsOrBestOffer;
            this.IsAcceptingTrades = auction.IsAcceptingTrades;
            this.CreatedAtString = auction.CreatedAt;
            this.UpdatedAtString = auction.UpdatedAt;
            this.ChatLineId = auction.MostRecentChatLineId;
        }
    }
}
