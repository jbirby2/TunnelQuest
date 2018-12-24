using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("size")]
    public class Size
    {
        [Key]
        [Column("size_code")]
        public string SizeCode { get; set; }
    }
}
