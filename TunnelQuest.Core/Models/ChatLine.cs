using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("chat_line")]
    public class ChatLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("chat_line_id")]
        public long ChatLineId { get; set; }

        [ForeignKey("AuthToken")]
        [Column("auth_token_id")]
        public short AuthTokenId { get; set; }
        public AuthToken AuthToken { get; set; }

        [Required]
        [ForeignKey("Server")]
        [Column("server_code")]
        public string ServerCode { get; set; }
        public Server Server { get; set; }

        [Required]
        [Column("player_name")]
        public string PlayerName { get; set; }

        [Required]
        [Column("text")]
        public string Text { get; set; }

        [Column("sent_at")]
        public DateTime SentAt { get; set; }

        public ICollection<Auction> Auctions { get; set; } = new List<Auction>();
    }
}
