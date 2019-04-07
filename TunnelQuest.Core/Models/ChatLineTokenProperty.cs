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
        [ForeignKey("ChatLine")]
        [Column("chat_line_id")]
        public long ChatLineId { get; set; }
        public ChatLine ChatLine { get; set; }

        // Index is used to keep the ordering correct, because apparently you cannot rely on Entity Framework to insert records in the same order
        // you added them to a collection.  This is causing chat_line_token records to get inserted in the wrong order often.
        // (It's a byte instead of an int to avoid wasting disk space since it's very very very unlikely that any chat line will ever have more than 255 tokens)
        [Column("token_index")]
        public byte TokenIndex { get; set; }

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
