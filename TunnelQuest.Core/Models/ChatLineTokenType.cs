using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("chat_line_token_type")]
    public class ChatLineTokenType
    {
        [Key]
        [Column("token_type_code")]
        public string TokenTypeCode { get; set; }

    }
}
