using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientAuction
    {
        public long Id { get; set; }
        public long? ReplacesAuctionId { get; set; }
        public string ItemName { get; set; }
        public string AliasText { get; set; }
        public bool IsKnownItem { get; set; }
        public bool IsBuying { get; set; }
        public int? Price { get; set; }
        public bool IsOrBestOffer { get; set; }
        public bool IsAcceptingTrades { get; set; }
        public bool IsPermanent { get; set; }
        public DateTime CreatedAtString { get; set; } // named String even though it's a DateTime in C# because it will be serialized as a string in javascript

        public bool PassesFilter { get; set; }


        public ClientAuction()
        {
        }

        public ClientAuction(Auction auction, bool passesFilter)
        {
            this.Id = auction.AuctionId;
            this.ReplacesAuctionId = auction.ReplacesAuctionId;
            this.ItemName = auction.ItemName;
            this.AliasText = auction.AliasText;
            this.IsKnownItem = auction.IsKnownItem;
            this.IsBuying = auction.IsBuying;
            this.Price = auction.Price;
            this.IsOrBestOffer = auction.IsOrBestOffer;
            this.IsAcceptingTrades = auction.IsAcceptingTrades;
            this.IsPermanent = auction.IsPermanent;
            this.CreatedAtString = auction.CreatedAt;
            this.PassesFilter = passesFilter;
        }
    }
}
