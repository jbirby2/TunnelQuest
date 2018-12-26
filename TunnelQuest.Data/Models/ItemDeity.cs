using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("item_deity")]
    public class ItemDeity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_deity_id")]
        public int ItemDeityId { get; set; }

        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Deity")]
        [Column("deity_name")]
        public string DeityName { get; set; }
        public Deity Deity { get; set; }
    }
}
