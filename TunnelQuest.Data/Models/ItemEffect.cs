using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("item_effect")]
    public class ItemEffect
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_effect_id")]
        public int ItemEffectId { get; set; }

        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Effect")]
        [Column("effect_name")]
        public string EffectName { get; set; }
        public Effect Effect { get; set; }

        [Column("effect_type_code")]
        public string EffectTypeCode { get; set; }
        public EffectType EffectType { get; set; }

        [Column("minimum_level")]
        public int? MinimumLevel { get; set; }

        [Column("casting_time")]
        public float? CastingTime { get; set; }
    }
}
