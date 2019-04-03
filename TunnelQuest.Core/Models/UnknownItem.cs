using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("unknown_item")]
    public class UnknownItem
    {
        [ForeignKey("Server")]
        [Column("server_code")]
        public string ServerCode { get; set; }
        public Server Server { get; set; }

        [Column("is_buying")]
        public bool IsBuying { get; set; }

        [Required]
        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
