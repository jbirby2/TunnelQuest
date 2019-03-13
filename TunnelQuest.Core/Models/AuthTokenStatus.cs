using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("auth_token_status")]
    public class AuthTokenStatus
    {
        [Key]
        [Column("auth_token_status_code")]
        public string AuthTokenStatusCode { get; set; }
    }
}
