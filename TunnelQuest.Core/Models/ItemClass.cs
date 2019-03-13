using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("item_class")]
    public class ItemClass
    {
        [Required]
        [ForeignKey("Item")]
        [Column("item_name")]
        public string ItemName { get; set; }
        public Item Item { get; set; }

        [Required]
        [ForeignKey("Class")]
        [Column("class_code")]
        public string ClassCode { get; set; }
        public Class Class { get; set; }
    }
}
