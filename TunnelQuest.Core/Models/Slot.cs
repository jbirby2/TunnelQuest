using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("slot")]
    public class Slot
    {
        [Key]
        [Column("slot_code")]
        public string SlotCode { get; set; }
    }
}
