using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("deity")]
    public class Deity
    {
        [Key]
        [Column("deity_name")]
        public string DeityName { get; set; }

    }
}
