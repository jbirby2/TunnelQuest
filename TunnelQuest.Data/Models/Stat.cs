﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("stat")]
    public class Stat
    {
        [Key]
        [Column("stat_code")]
        public string StatCode { get; set; }

    }
}
