using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("spell_source")]
    public class SpellSource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("spell_source_id")]
        public int SpellSourceId { get; set; }

        [Required]
        [ForeignKey("Spell")]
        [Column("spell_name")]
        public string SpellName { get; set; }
        public Spell Spell { get; set; }

        [Required]
        [Column("text")]
        public string Text { get; set; }
    }
}
