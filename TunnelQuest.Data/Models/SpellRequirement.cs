using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("spell_requirement")]
    public class SpellRequirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("spell_requirement_id")]
        public int SpellRequirementId { get; set; }

        [ForeignKey("Spell")]
        [Column("spell_name")]
        public string SpellName { get; set; }
        public Spell Spell { get; set; }

        [ForeignKey("Class")]
        [Column("class_code")]
        public string ClassCode { get; set; }
        public Class Class { get; set; }

        [Column("required_level")]
        public int RequiredLevel { get; set; }
    }
}
