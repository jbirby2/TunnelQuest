using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("class")]
    public class Class
    {
        [Key]
        [Column("class_code")]
        public string ClassCode { get; set; }

        [Column("class_name")]
        public string ClassName { get; set; }
    }
}
