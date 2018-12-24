using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    public class ItemStat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemStatId { get; set; }

        [ForeignKey("Item")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Stat")]
        public string StatCode { get; set; }
        public Stat Stat { get; set; }

        public int Adjustment { get; set; }
    }
}
