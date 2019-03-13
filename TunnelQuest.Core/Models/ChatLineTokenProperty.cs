using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("chat_line_token_property")]
    public class ChatLineTokenProperty
    {
        [ForeignKey("ChatLineToken")]
        [Column("chat_line_token_id")]
        public long ChatLineTokenId { get; set; }
        public ChatLineToken ChatLineToken { get; set; }

        [Required]
        [Column("property")]
        public string Property { get; set; }

        [Required]
        [Column("value")]
        public string Value { get; set; }

        public void CopyValuesFrom(ChatLineTokenProperty prop)
        {
            this.Property = prop.Property;
            this.Value = prop.Value;
        }
    }
}
