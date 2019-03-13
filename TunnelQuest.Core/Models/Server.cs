using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("server")]
    public class Server
    {
        [Key]
        [Column("server_code")]
        public string ServerCode { get; set; }
        
        [Required]
        [Column("server_name")]
        public string ServerName { get; set; }
    }
}
