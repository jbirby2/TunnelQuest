using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("item_deity")]
    public class ItemDeity
    {
        [Required]
        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [Required]
        [ForeignKey("Deity")]
        [Column("deity_name")]
        public string DeityName { get; set; }
        public Deity Deity { get; set; }
    }
}
