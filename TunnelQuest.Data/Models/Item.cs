using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TunnelQuest.Data.Models
{
    public class Item
    {
        [Key]
        public string ItemName { get; set; }

        public bool IsMagic { get; set; }
        public bool IsLore { get; set; }
        public bool IsTemporary { get; set; }
        public float Weight { get; set; }
        public Size Size { get; set; }
        public ICollection<Race> Races { get; set; }
        public ICollection<Class> Classes { get; set; }
        public ICollection<ItemStat> ItemStats { get; set; }
        public ICollection<ItemEffect> ItemEffects { get; set; }
    }
}
