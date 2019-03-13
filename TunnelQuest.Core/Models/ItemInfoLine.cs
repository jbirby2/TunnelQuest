using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("item_info_line")]
    public class ItemInfoLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_info_line_id")]
        public int ItemInfoLineId { get; set; }

        [Required]
        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [Required]
        [Column("text")]
        public string Text { get; set; }
    }
}
