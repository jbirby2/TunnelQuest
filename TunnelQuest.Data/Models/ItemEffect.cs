using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    public enum ItemEffectType { Worn, AnySlot, MustEquip }

    public class ItemEffect
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemEffectId { get; set; }

        [ForeignKey("Item")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Effect")]
        public string EffectName { get; set; }
        public Effect Effect { get; set; }

        public ItemEffectType EffectType { get; set; }
        public int? RequiredLevel { get; set; }
        public float? CastingTime { get; set; }
    }
}
