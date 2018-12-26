using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Data.Models
{
    [Table("item")]
    public class Item
    {
        [Key]
        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("icon_file_name")]
        public string IconFileName { get; set; }

        [Column("is_magic")]
        public bool IsMagic { get; set; }

        [Column("is_lore")]
        public bool IsLore { get; set; }

        [Column("is_no_drop")]
        public bool IsNoDrop { get; set; }

        [Column("is_temporary")]
        public bool IsTemporary { get; set; }

        [Column("is_quest_item")]
        public bool IsQuestItem { get; set; }

        [Column("weight")]
        public float Weight { get; set; }

        [ForeignKey("Size")]
        [Column("size_code")]
        public string SizeCode { get; set; }
        public Size Size { get; set; }

        public ICollection<ItemRace> ItemRaces { get; set; }
        public ICollection<ItemClass> ItemClasses { get; set; }
        public ICollection<ItemStat> ItemStats { get; set; }
        public ICollection<ItemEffect> ItemEffects { get; set; }


        // equipment

        public ICollection<ItemSlot> ItemSlots { get; set; }
        public ICollection<ItemDeity> Deities { get; set; }


        // weapons

        [ForeignKey("WeaponSkill")]
        [Column("weapon_skill_code")]
        public string WeaponSkillCode { get; set; }
        public WeaponSkill WeaponSkill { get; set; }

        [Column("attack_damage")]
        public int? AttackDamage { get; set; }

        [Column("attack_delay")]
        public int? AttackDelay { get; set; }


        // containers

        [Column("capacity")]
        public int? Capacity { get; set; }

        [ForeignKey("CapacitySize")]
        [Column("capacity_size_code")]
        public string CapacitySizeCode { get; set; }
        public Size CapacitySize { get; set; }

        [Column("weight_reduction")]
        public float? WeightReduction { get; set; }


        // consumables

        [Column("is_expendable")]
        public bool IsExpendable { get; set; }

        [Column("max_charges")]
        public int? MaxCharges { get; set; }
    }
}
