using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Data;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientItem
    {
        // static

        public static ClientItem[] Create(Item[] items)
        {
            var result = new ClientItem[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                result[i] = new ClientItem(items[i]);
            }
            return result;
        }


        // non-static

        public string ItemName { get; set; }
        public string IconFileName { get; set; }
        public bool IsMagic { get; set; }
        public bool IsLore { get; set; }
        public bool IsNoDrop { get; set; }
        public bool IsNoTrade { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsQuestItem { get; set; }
        public bool IsArtifact { get; set; }
        public int? RequiredLevel { get; set; }
        public float Weight { get; set; }
        public string SizeCode { get; set; }

        // stats

        public int? Strength { get; set; }
        public int? Stamina { get; set; }
        public int? Agility { get; set; }
        public int? Dexterity { get; set; }
        public int? Wisdom { get; set; }
        public int? Intelligence { get; set; }
        public int? Charisma { get; set; }
        public int? HitPoints { get; set; }
        public int? Mana { get; set; }
        public int? ArmorClass { get; set; }
        public int? MagicResist { get; set; }
        public int? PoisonResist { get; set; }
        public int? DiseaseResist { get; set; }
        public int? FireResist { get; set; }
        public int? ColdResist { get; set; }
        public float? Haste { get; set; }

        // bard instruments

        public int? SingingModifier { get; set; }
        public int? PercussionModifier { get; set; }
        public int? StringedModifier { get; set; }
        public int? BrassModifier { get; set; }
        public int? WindModifier { get; set; }

        // spell effect

        public string EffectSpellName { get; set; }
        public string EffectTypeCode { get; set; }
        public int? EffectMinimumLevel { get; set; }
        public float? EffectCastingTime { get; set; }

        // weapons

        public string WeaponSkillCode { get; set; }
        public int? AttackDamage { get; set; }
        public int? AttackDelay { get; set; }
        public int? Range { get; set; }

        // containers

        public int? Capacity { get; set; }
        public string CapacitySizeCode { get; set; }
        public float? WeightReduction { get; set; }

        // consumables

        public bool? IsExpendable { get; set; }
        public int? MaxCharges { get; set; }


        // relationships

        public string[] Races { get; set; }
        public string[] Classes { get; set; }
        public string[] Slots { get; set; }
        public string[] Deities { get; set; }
        public string[] Info { get; set; }



        public ClientItem()
        {
        }

        public ClientItem(Item item)
        {
            this.ItemName = item.ItemName;
            this.IconFileName = item.IconFileName;
            this.IsMagic = item.IsMagic;
            this.IsLore = item.IsLore;
            this.IsNoDrop = item.IsNoDrop;
            this.IsNoTrade = item.IsNoTrade;
            this.IsTemporary = item.IsTemporary;
            this.IsQuestItem = item.IsQuestItem;
            this.IsArtifact = item.IsArtifact;
            this.RequiredLevel = item.RequiredLevel;
            this.Weight = item.Weight;
            this.SizeCode = item.SizeCode;

            // stats

            this.Strength = item.Strength;
            this.Stamina = item.Stamina;
            this.Agility = item.Agility;
            this.Dexterity = item.Dexterity;
            this.Wisdom = item.Wisdom;
            this.Intelligence = item.Intelligence;
            this.Charisma = item.Charisma;
            this.HitPoints = item.HitPoints;
            this.Mana = item.Mana;
            this.ArmorClass = item.ArmorClass;
            this.MagicResist = item.MagicResist;
            this.PoisonResist = item.PoisonResist;
            this.DiseaseResist = item.DiseaseResist;
            this.FireResist = item.FireResist;
            this.ColdResist = item.ColdResist;
            this.Haste = item.Haste;

            // bard instruments

            this.SingingModifier = item.SingingModifier;
            this.PercussionModifier = item.PercussionModifier;
            this.StringedModifier = item.StringedModifier;
            this.BrassModifier = item.BrassModifier;
            this.WindModifier = item.WindModifier;

            // spell effect

            this.EffectSpellName = item.EffectSpellName;
            this.EffectTypeCode = item.EffectTypeCode;
            this.EffectMinimumLevel = item.EffectMinimumLevel;
            this.EffectCastingTime = item.EffectCastingTime;

            // weapons

            this.WeaponSkillCode = item.WeaponSkillCode;
            this.AttackDamage = item.AttackDamage;
            this.AttackDelay = item.AttackDelay;
            this.Range = item.Range;

            // containers

            this.Capacity = item.Capacity;
            this.CapacitySizeCode = item.CapacitySizeCode;
            this.WeightReduction = item.WeightReduction;

            // consumables

            this.IsExpendable = item.IsExpendable;
            this.MaxCharges = item.MaxCharges;

            // relationships

            int i;

            if (item.Races.Count > 0)
            {
                if (item.Races.Count == RaceCodes.All.Count())
                    this.Races = new string[1] { "ALL" };
                else
                {
                    i = 0;
                    this.Races = new string[item.Races.Count];
                    foreach (var race in item.Races)
                    {
                        this.Races[i] = race.RaceCode;
                        i++;
                    }
                }
            }


            if (item.Classes.Count > 0)
            {
                if (item.Classes.Count == ClassCodes.All.Count())
                    this.Classes = new string[1] { "ALL" };
                else
                {
                    i = 0;
                    this.Classes = new string[item.Classes.Count];
                    foreach (var classs in item.Classes)
                    {
                        this.Classes[i] = classs.ClassCode;
                        i++;
                    }
                }
            }

            if (item.Slots.Count > 0)
            {
                i = 0;
                this.Slots = new string[item.Slots.Count];
                foreach (var slot in item.Slots)
                {
                    this.Slots[i] = slot.SlotCode;
                    i++;
                }
            }

            if (item.Deities.Count > 0)
            {
                i = 0;
                this.Deities = new string[item.Deities.Count];
                foreach (var deity in item.Deities)
                {
                    this.Deities[i] = deity.DeityName;
                    i++;
                }
            }

            if (item.Info.Count > 0)
            {
                i = 0;
                this.Info = new string[item.Info.Count];
                foreach (var info in item.Info)
                {
                    this.Info[i] = info.Text;
                    i++;
                }
            }
            
        }
    }
}
