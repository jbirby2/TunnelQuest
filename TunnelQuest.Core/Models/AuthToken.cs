using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("auth_token")]
    public class AuthToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("auth_token_id")]
        public short AuthTokenId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("value")]
        public string Value { get; set; }

        [Required]
        [ForeignKey("AuthTokenStatus")]
        [Column("auth_token_status_code")]
        public string AuthTokenStatusCode { get; set; }
        public AuthTokenStatus AuthTokenStatus { get; set; }
    }
}
