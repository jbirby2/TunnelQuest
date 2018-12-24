using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("item_race")]
    public class ItemRace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("item_race_id")]
        public int ItemRaceId { get; set; }

        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [ForeignKey("Race")]
        [Column("race_code")]
        public string RaceCode { get; set; }
        public Race Race { get; set; }
    }
}
