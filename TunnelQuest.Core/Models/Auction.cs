﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("auction")]
    public class Auction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("auction_id")]
        public long AuctionId { get; set; }

        // denormalized to this table for performance reasons (indexes)
        [Required]
        [ForeignKey("Server")]
        [Column("server_code")]
        public string ServerCode { get; set; }
        public Server Server { get; set; }

        // denormalized to this table for performance reasons (indexes)
        [Required]
        [Column("player_name")]
        public string PlayerName { get; set; }

        [ForeignKey("PreviousAuction")]
        [Column("previous_auction_id")]
        public long? PreviousAuctionId { get; set; }
        public Auction PreviousAuction { get; set; }

        [ForeignKey("MostRecentChatLine")]
        [Column("most_recent_chat_line_id")]
        public long MostRecentChatLineId { get; set; }
        public ChatLine MostRecentChatLine { get; set; }

        // ItemName is intentionally NOT declared a ForeignKey here, because ItemName
        // might also be a text string that matches no known item (e.g. "jboots mq")
        [Required]
        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("is_known_item")]
        public bool IsKnownItem { get; set; }

        [Column("is_buying")]
        public bool IsBuying { get; set; }  // "WTB"
        
        [Column("price")]
        public int? Price { get; set; }

        [Column("is_or_best_offer")]
        public bool IsOrBestOffer { get; set; } // "OBO"

        [Column("is_accepting_trades")]
        public bool IsAcceptingTrades { get; set; } // "WTT", "WTTF"

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }


        public Auction()
        {
        }

        public Auction(Auction auctionToCopy, DateTime timestamp)
        {
            this.CopyValuesFrom(auctionToCopy);
            this.CreatedAt = timestamp;
            this.UpdatedAt = timestamp;
        }

        public override bool Equals(object obj)
        {
            if (obj is Auction)
                return this.Equals((Auction)obj);
            else
                return false;
        }

        public bool Equals(Auction auction)
        {
            return (this.IsBuying == auction.IsBuying
                && this.ItemName == auction.ItemName
                && this.IsKnownItem == auction.IsKnownItem
                && this.Price == auction.Price
                && this.IsOrBestOffer == auction.IsOrBestOffer
                && this.IsAcceptingTrades == auction.IsAcceptingTrades);
        }

        public void CopyValuesFrom(Auction auction)
        {
            this.IsBuying = auction.IsBuying;
            this.ItemName = auction.ItemName;
            this.IsKnownItem = auction.IsKnownItem;
            this.Price = auction.Price;
            this.IsOrBestOffer = auction.IsOrBestOffer;
            this.IsAcceptingTrades = auction.IsAcceptingTrades;
        }
    }
}