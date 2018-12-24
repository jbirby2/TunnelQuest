﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("item_stat")]
    public class ItemStat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_stat_id")]
        public int ItemStatId { get; set; }

        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Stat")]
        [Column("stat_code")]
        public string StatCode { get; set; }
        public Stat Stat { get; set; }

        [Column("adjustment")]
        public int Adjustment { get; set; }
    }
}
