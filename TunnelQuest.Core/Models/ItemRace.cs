using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("item_race")]
    public class ItemRace
    {
        [Required]
        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [Required]
        [ForeignKey("Race")]
        [Column("race_code")]
        public string RaceCode { get; set; }
        public Race Race { get; set; }
    }
}
