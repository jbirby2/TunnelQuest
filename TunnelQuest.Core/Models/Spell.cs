using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("spell")]
    public class Spell
    {
        [Key]
        [Column("spell_name")]
        public string SpellName { get; set; }

        [Column("icon_file_name")]
        public string IconFileName { get; set; }

        [Column("description")]
        public string Description { get; set; }


        // relationships

        public ICollection<SpellRequirement> Requirements { get; set; } = new List<SpellRequirement>();
        public ICollection<SpellEffectDetail> EffectDetails { get; set; } = new List<SpellEffectDetail>();
        public ICollection<SpellSource> Sources { get; set; } = new List<SpellSource>();
    }
}
