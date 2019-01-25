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

        [Column("is_no_trade")]
        public bool IsNoTrade { get; set; }

        [Column("is_temporary")]
        public bool IsTemporary { get; set; }

        [Column("is_quest_item")]
        public bool IsQuestItem { get; set; }

        [Column("is_artifact")]
        public bool IsArtifact { get; set; }

        [Column("required_level")]
        public int? RequiredLevel { get; set; }

        [Column("weight")]
        public float Weight { get; set; }

        [ForeignKey("Size")]
        [Column("size_code")]
        public string SizeCode { get; set; }
        public Size Size { get; set; }

        
        // stats

        [Column("strength")]
        public int? Strength { get; set; }

        [Column("stamina")]
        public int? Stamina { get; set; }

        [Column("agility")]
        public int? Agility { get; set; }

        [Column("dexterity")]
        public int? Dexterity { get; set; }

        [Column("wisdom")]
        public int? Wisdom { get; set; }

        [Column("intelligence")]
        public int? Intelligence { get; set; }

        [Column("charisma")]
        public int? Charisma { get; set; }

        [Column("hit_points")]
        public int? HitPoints { get; set; }

        [Column("mana")]
        public int? Mana { get; set; }

        [Column("armor_class")]
        public int? ArmorClass { get; set; }

        [Column("magic_resist")]
        public int? MagicResist { get; set; }

        [Column("poison_resist")]
        public int? PoisonResist { get; set; }

        [Column("disease_resist")]
        public int? DiseaseResist { get; set; }

        [Column("fire_resist")]
        public int? FireResist { get; set; }

        [Column("cold_resist")]
        public int? ColdResist { get; set; }

        [Column("haste")]
        public float? Haste { get; set; }

        // bard instruments

        [Column("singing_modifier")]
        public int? SingingModifier { get; set; }

        [Column("percussion_modifier")]
        public int? PercussionModifier { get; set; }

        [Column("stringed_modifier")]
        public int? StringedModifier { get; set; }

        [Column("brass_modifier")]
        public int? BrassModifier { get; set; }

        [Column("wind_modifier")]
        public int? WindModifier { get; set; }

        // spell effect

        [ForeignKey("EffectSpell")]
        [Column("effect_spell_name")]
        public string EffectSpellName { get; set; }
        public Spell EffectSpell { get; set; }

        [ForeignKey("EffectType")]
        [Column("effect_type_code")]
        public string EffectTypeCode { get; set; }
        public EffectType EffectType { get; set; }

        [Column("effect_minimum_level")]
        public int? EffectMinimumLevel { get; set; }

        [Column("effect_casting_time")]
        public float? EffectCastingTime { get; set; }

        // weapons

        [ForeignKey("WeaponSkill")]
        [Column("weapon_skill_code")]
        public string WeaponSkillCode { get; set; }
        public WeaponSkill WeaponSkill { get; set; }

        [Column("attack_damage")]
        public int? AttackDamage { get; set; }

        [Column("attack_delay")]
        public int? AttackDelay { get; set; }

        [Column("range")]
        public int? Range { get; set; }

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
        public bool? IsExpendable { get; set; }

        [Column("max_charges")]
        public int? MaxCharges { get; set; }


        // relationships

        public ICollection<ItemRace> Races { get; set; } = new List<ItemRace>();
        public ICollection<ItemClass> Classes { get; set; } = new List<ItemClass>();
        public ICollection<ItemSlot> Slots { get; set; } = new List<ItemSlot>();
        public ICollection<ItemDeity> Deities { get; set; } = new List<ItemDeity>();
        public ICollection<ItemInfoLine> Info { get; set; } = new List<ItemInfoLine>();
    }
}
