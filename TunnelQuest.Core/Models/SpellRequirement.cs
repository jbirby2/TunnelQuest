using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("spell_requirement")]
    public class SpellRequirement
    {
        [Required]
        [ForeignKey("Spell")]
        [Column("spell_name")]
        public string SpellName { get; set; }
        public Spell Spell { get; set; }

        [Required]
        [ForeignKey("Class")]
        [Column("class_code")]
        public string ClassCode { get; set; }
        public Class Class { get; set; }

        [Column("required_level")]
        public int RequiredLevel { get; set; }
    }
}
