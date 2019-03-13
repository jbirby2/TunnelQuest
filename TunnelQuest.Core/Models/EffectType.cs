using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("effect_type")]
    public class EffectType
    {
        [Key]
        [Column("effect_type_code")]
        public string EffectTypeCode { get; set; }
    }
}
