using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TunnelQuest.Data.Models
{
    public class Effect
    {
        [Key]
        public string EffectName { get; set; }
    }
}
