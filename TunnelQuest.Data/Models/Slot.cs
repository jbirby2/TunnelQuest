using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TunnelQuest.Data.Models
{
    public class Slot
    {
        [Key]
        public string SlotName { get; set; }
    }
}
