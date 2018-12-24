using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("effect")]
    public class Effect
    {
        [Key]
        [Column("effect_name")]
        public string EffectName { get; set; }
    }
}
