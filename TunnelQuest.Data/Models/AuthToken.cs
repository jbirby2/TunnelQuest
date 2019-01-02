using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("auth_token")]
    public class AuthToken
    {
        [Key]
        [Column("token_name")]
        public string TokenName { get; set; }

        [Column("value")]
        public string Value { get; set; }
    }
}
