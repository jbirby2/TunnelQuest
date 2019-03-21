using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("alias")]
    public class Alias
    {
        [Key]
        [Required]
        [Column("alias_text")]
        public string AliasText { get; set; }

        // not a foreign key because it could be an unknown item name (i.e. "jboots mq")
        [Required]
        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
