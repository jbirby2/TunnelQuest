using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("item_slot")]
    public class ItemSlot
    {
        [Required]
        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [Required]
        [ForeignKey("Slot")]
        [Column("slot_code")]
        public string SlotCode { get; set; }
        public Slot Slot { get; set; }
    }
}
