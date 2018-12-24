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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_slot_id")]
        public int ItemSlotId { get; set; }

        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Slot")]
        [Column("slot_code")]
        public string SlotCode { get; set; }
        public Slot Slot { get; set; }
    }
}
