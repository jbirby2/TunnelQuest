using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("chat_line_auction")]
    public class ChatLineAuction
    {
        [ForeignKey("ChatLine")]
        [Column("chat_line_id")]
        public long ChatLineId { get; set; }
        public ChatLine ChatLine { get; set; }

        [ForeignKey("Auction")]
        [Column("auction_id")]
        public long AuctionId { get; set; }
        public Auction Auction { get; set; }
    }
}
