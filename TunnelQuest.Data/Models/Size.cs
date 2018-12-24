using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TunnelQuest.Data.Models
{
    public class Size
    {
        [Key]
        public string SizeName { get; set; }
    }
}
