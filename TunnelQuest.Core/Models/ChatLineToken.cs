using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("chat_line_token")]
    public class ChatLineToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("chat_line_token_id")]
        public long ChatLineTokenId { get; set; }

        [Required]
        [ForeignKey("ChatLine")]
        [Column("chat_line_id")]
        public long ChatLineId { get; set; }
        public ChatLine ChatLine { get; set; }

        [Required]
        [ForeignKey("TokenType")]
        [Column("token_type_code")]
        public string TokenTypeCode { get; set; }
        public ChatLineTokenType TokenType { get; set; }

        public ICollection<ChatLineTokenProperty> Properties { get; set; } = new List<ChatLineTokenProperty>();

        public ChatLineToken()
        {
        }

        public ChatLineToken(ChatLineToken tokenToCopy)
        {
            this.CopyValuesFrom(tokenToCopy);
        }

        public void CopyValuesFrom(ChatLineToken token)
        {
            this.TokenTypeCode = token.TokenTypeCode;

            this.Properties.Clear();
            foreach (var prop in token.Properties)
            {
                var myProp = new ChatLineTokenProperty()
                {
                    ChatLineTokenId = this.ChatLineTokenId,
                    ChatLineToken = this
                };
                myProp.CopyValuesFrom(prop);
            }
        }
    }
}
