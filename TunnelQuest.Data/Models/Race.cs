using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("race")]
    public class Race
    {
        [Key]
        [Column("race_code")]
        public string RaceCode { get; set; }

        [Column("race_name")]
        public string RaceName { get; set; }
    }
}
