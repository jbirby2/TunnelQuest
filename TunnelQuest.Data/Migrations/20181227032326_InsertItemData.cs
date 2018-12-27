using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data.Migrations.Data;
using TunnelQuest.Data.Models;

namespace TunnelQuest.Data.Migrations
{
    public partial class InsertItemData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = new TunnelQuestContext())
            {
                insertClasses(context);
                insertRaces(context);
                insertSizes(context);
                insertSlots(context);
                insertEffectTypes(context);
                insertWeaponSkills(context);
                insertStats(context);
                insertDeities(context);
                var items = insertItems(context);
                var effectNameNormalizer = insertEffects(context, items);

                // use effectNameNormalizer to make sure all items use the most popular spelling of their effect names
                foreach (var item in items)
                {
                    if (item.ItemEffects != null)
                    {
                        foreach (var effect in item.ItemEffects)
                        {
                            effect.EffectName = effectNameNormalizer[effect.EffectName.ToLower()];
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var context = new TunnelQuestContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var items = deleteItems(context);
                        context.SaveChanges();

                        deleteEffects(context, items);

                        deleteClasses(context);
                        deleteRaces(context);
                        deleteSizes(context);
                        deleteSlots(context);
                        deleteEffectTypes(context);
                        deleteWeaponSkills(context);
                        deleteStats(context);
                        deleteDeities(context);

                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        // private helpers

        #region class
        private void insertClasses(TunnelQuestContext context)
        {
            context.AddRange(getClasses());
        }

        private void deleteClasses(TunnelQuestContext context)
        {
            context.RemoveRange(getClasses());
        }

        private IEnumerable<Class> getClasses()
        {
            return new Class[] {
                new Class() {
                    ClassCode = "ENC",
                    ClassName = "Enchanter"
                },
                new Class()
                {
                    ClassCode = "MAG",
                    ClassName = "Magician"
                },
                new Class()
                {
                    ClassCode = "NEC",
                    ClassName = "Necromancer"
                },
                new Class()
                {
                    ClassCode = "WIZ",
                    ClassName = "Wizard"
                },
                new Class()
                {
                    ClassCode = "CLR",
                    ClassName = "Cleric"
                },
                new Class()
                {
                    ClassCode = "DRU",
                    ClassName = "Druid"
                },
                new Class()
                {
                    ClassCode = "SHM",
                    ClassName = "Shaman"
                },
                new Class()
                {
                    ClassCode = "BRD",
                    ClassName = "Bard"
                },
                new Class()
                {
                    ClassCode = "MNK",
                    ClassName = "Monk"
                },
                new Class()
                {
                    ClassCode = "RNG",
                    ClassName = "Ranger"
                },
                new Class()
                {
                    ClassCode = "ROG",
                    ClassName = "Rogue"
                },
                new Class()
                {
                    ClassCode = "PAL",
                    ClassName = "Paladin"
                },
                new Class()
                {
                    ClassCode = "SHD",
                    ClassName = "Shadow Knight"
                },
                new Class()
                {
                    ClassCode = "WAR",
                    ClassName = "Warrior"
                }
            };
        }

        #endregion

        #region race
        private void insertRaces(TunnelQuestContext context)
        {
            context.AddRange(getRaces());
        }

        private void deleteRaces(TunnelQuestContext context)
        {
            context.RemoveRange(getRaces());
        }

        private IEnumerable<Race> getRaces()
        {
            return new Race[] {
                new Race() {
                    RaceCode = "BAR",
                    RaceName = "Barbarian"
                },
                new Race() {
                    RaceCode = "DEF",
                    RaceName = "Dark Elf"
                },
                new Race() {
                    RaceCode = "DWF",
                    RaceName = "Dwarf"
                },
                new Race() {
                    RaceCode = "ERU",
                    RaceName = "Erudite"
                },
                new Race() {
                    RaceCode = "GNM",
                    RaceName = "Gnome"
                },
                new Race() {
                    RaceCode = "HEF",
                    RaceName = "Half-Elf"
                },
                new Race() {
                    RaceCode = "HFL",
                    RaceName = "Halfling"
                },
                new Race() {
                    RaceCode = "HIE",
                    RaceName = "High Elf"
                },
                new Race() {
                    RaceCode = "HUM",
                    RaceName = "Human"
                },
                new Race() {
                    RaceCode = "IKS",
                    RaceName = "Iksar"
                },
                new Race() {
                    RaceCode = "OGR",
                    RaceName = "Ogre"
                },
                new Race() {
                    RaceCode = "TRL",
                    RaceName = "Troll"
                },
                new Race() {
                    RaceCode = "ELF",
                    RaceName = "Wood Elf"
                },
            };
        }

        #endregion

        #region size
        private void insertSizes(TunnelQuestContext context)
        {
            context.AddRange(getSizes());
        }

        private void deleteSizes(TunnelQuestContext context)
        {
            context.RemoveRange(getSizes());
        }

        private IEnumerable<Size> getSizes()
        {
            return new Size[] {
                new Size() {
                    SizeCode = "TINY"
                },
                new Size() {
                    SizeCode = "SMALL"
                },
                new Size() {
                    SizeCode = "MEDIUM"
                },
                new Size() {
                    SizeCode = "LARGE"
                },
                new Size() {
                    SizeCode = "GIANT"
                }
            };
        }
        #endregion

        #region slot
        private void insertSlots(TunnelQuestContext context)
        {
            context.AddRange(getSlots());
        }

        private void deleteSlots(TunnelQuestContext context)
        {
            context.RemoveRange(getSlots());
        }

        private IEnumerable<Slot> getSlots()
        {
            return new Slot[] {
                new Slot() {
                    SlotCode = "ARMS"
                },
                new Slot() {
                    SlotCode = "BACK"
                },
                new Slot() {
                    SlotCode = "CHEST"
                },
                new Slot() {
                    SlotCode = "EAR"
                },
                new Slot() {
                    SlotCode = "FACE"
                },
                new Slot() {
                    SlotCode = "FEET"
                },
                new Slot() {
                    SlotCode = "FINGER"
                },
                new Slot() {
                    SlotCode = "HANDS"
                },
                new Slot() {
                    SlotCode = "HEAD"
                },
                new Slot() {
                    SlotCode = "LEGS"
                },
                new Slot() {
                    SlotCode = "NECK"
                },
                new Slot() {
                    SlotCode = "SHOULDERS"
                },
                new Slot() {
                    SlotCode = "WAIST"
                },
                new Slot() {
                    SlotCode = "WRIST"
                },
                new Slot() {
                    SlotCode = "PRIMARY"
                },
                new Slot() {
                    SlotCode = "SECONDARY"
                },
                new Slot() {
                    SlotCode = "RANGE"
                },
                new Slot() {
                    SlotCode = "AMMO"
                }
            };
        }
        #endregion

        #region effect_type
        private void insertEffectTypes(TunnelQuestContext context)
        {
            context.AddRange(getEffectTypes());
        }

        private void deleteEffectTypes(TunnelQuestContext context)
        {
            context.RemoveRange(getEffectTypes());
        }

        private IEnumerable<EffectType> getEffectTypes()
        {
            return new EffectType[] {
                new EffectType() {
                    EffectTypeCode = "Combat"
                },
                new EffectType() {
                    EffectTypeCode = "Worn"
                },
                new EffectType() {
                    EffectTypeCode = "ClickAnySlot"
                },
                new EffectType() {
                    EffectTypeCode = "ClickEquipped"
                }
            };
        }
        #endregion

        #region weapon_skill
        private void insertWeaponSkills(TunnelQuestContext context)
        {
            context.AddRange(getWeaponSkills());
        }

        private void deleteWeaponSkills(TunnelQuestContext context)
        {
            context.RemoveRange(getWeaponSkills());
        }

        private IEnumerable<WeaponSkill> getWeaponSkills()
        {
            return new WeaponSkill[] {
                new WeaponSkill() {
                    WeaponSkillCode = "1H Blunt"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "2H Blunt"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "1H Slashing"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "2H Slashing"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "Piercing"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "2H Piercing"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "Archery"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "Throwingv2"
                },
                new WeaponSkill() {
                    WeaponSkillCode = "Hand to Hand"
                }
            };
        }
        #endregion

        #region stat
        private void insertStats(TunnelQuestContext context)
        {
            context.AddRange(getStats());
        }

        private void deleteStats(TunnelQuestContext context)
        {
            context.RemoveRange(getStats());
        }

        private IEnumerable<Stat> getStats()
        {
            return new Stat[] {
                new Stat() {
                    StatCode = "STR"
                },
                new Stat() {
                    StatCode = "STA"
                },
                new Stat() {
                    StatCode = "AGI"
                },
                new Stat() {
                    StatCode = "DEX"
                },
                new Stat() {
                    StatCode = "WIS"
                },
                new Stat() {
                    StatCode = "INT"
                },
                new Stat() {
                    StatCode = "CHA"
                },
                new Stat() {
                    StatCode = "HP"
                },
                new Stat() {
                    StatCode = "MANA"
                },
                new Stat() {
                    StatCode = "AC"
                },
                new Stat() {
                    StatCode = "SV MAGIC"
                },
                new Stat() {
                    StatCode = "SV POISON"
                },
                new Stat() {
                    StatCode = "SV DISEASE"
                },
                new Stat() {
                    StatCode = "SV FIRE"
                },
                new Stat() {
                    StatCode = "SV COLD"
                }
            };
        }
        #endregion

        #region deity
        private void insertDeities(TunnelQuestContext context)
        {
            context.AddRange(getDeities());
        }

        private void deleteDeities(TunnelQuestContext context)
        {
            context.RemoveRange(getDeities());
        }

        private IEnumerable<Deity> getDeities()
        {
            return new Deity[] {
                new Deity() {
                    DeityName = "Cazic Thule"
                },
                new Deity() {
                    DeityName = "Tunare"
                },
                new Deity() {
                    DeityName = "Karana"
                },
                new Deity() {
                    DeityName = "Brell Serilis"
                },
                new Deity() {
                    DeityName = "Innoruuk"
                },
                new Deity() {
                    DeityName = "Quellious"
                },
                new Deity() {
                    DeityName = "Bertoxxulous"
                },
                new Deity() {
                    DeityName = "Erollisi Marr"
                },
                new Deity() {
                    DeityName = "Bristlebane"
                },
                new Deity() {
                    DeityName = "Mithaniel Marr"
                },
                new Deity() {
                    DeityName = "Prexus"
                },
                new Deity() {
                    DeityName = "Rallos Zek"
                },
                new Deity() {
                    DeityName = "Rodcet Nife"
                },
                new Deity() {
                    DeityName = "Solusek Ro"
                },
                new Deity() {
                    DeityName = "The Tribunal"
                }
            };
        }
        #endregion

        #region item

        private IEnumerable<Item> insertItems(TunnelQuestContext context)
        {
            var wikiData = WikiItemData.ReadFromEmbeddedResource();
            var items = parseWikiItems(wikiData);

            context.AddRange(items);

            return items;
        }

        private IEnumerable<Item> deleteItems(TunnelQuestContext context)
        {
            var wikiData = WikiItemData.ReadFromEmbeddedResource();
            var items = parseWikiItems(wikiData);

            // create a list of items with ONLY the names for use as deletion keys, and let the
            // cascading deletes on the foreign keys handle the deletions in the child tables
            var itemsToDelete = new Item[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                itemsToDelete[i] = new Item() { ItemName = items[i].ItemName };
            }
            context.RemoveRange(itemsToDelete);

            return items;
        }
        #endregion

        #region effect
        private Dictionary<string, string> insertEffects(TunnelQuestContext context, IEnumerable<Item> items)
        {
            var normalizer = getEffectNameNormalizer(items);

            foreach (string effectName in normalizer.Values)
            {
                context.Add(new Effect()
                {
                    EffectName = effectName
                });
            }

            return normalizer;
        }

        private Dictionary<string, string> deleteEffects(TunnelQuestContext context, IEnumerable<Item> items)
        {
            var normalizer = getEffectNameNormalizer(items);

            foreach (string effectName in normalizer.Values)
            {
                context.Remove(new Effect()
                {
                    EffectName = effectName
                });
            }

            return normalizer;
        }

        private Dictionary<string, string> getEffectNameNormalizer(IEnumerable<Item> items)
        {
            // One entry per effect name.
            // Usage: effectNameNormalizer[lowerCaseName] = nameToUseInDatabase
            var effectNameNormalizer = new Dictionary<string, string>();

            // build effectNameNormalizer

            var effectNameGroups = items
                .Where(item => item.ItemEffects != null && item.ItemEffects.Count > 0)
                .Select(item => item.ItemEffects.Select(ie => ie).Select(ie => ie.EffectName).ToArray())
                .SelectMany(names => names)
                .GroupBy(name => name.ToLower());

            foreach (var lowerNameGroup in effectNameGroups)
            {
                string lowerName = lowerNameGroup.Key;

                var mostPopularNameCase = lowerNameGroup
                    .GroupBy(name => name)
                    .OrderByDescending(nameGroup => nameGroup.Count())
                    .FirstOrDefault()
                    .Key;

                effectNameNormalizer[lowerName] = mostPopularNameCase;
            }

            return effectNameNormalizer;
        }
        #endregion

        private List<Item> parseWikiItems(IEnumerable<WikiItemData> wikiData)
        {
            var items = new List<Item>();

            foreach (WikiItemData wikiItem in wikiData)
            {
                if (wikiItem.Stats != null && !String.IsNullOrWhiteSpace(wikiItem.ItemName))
                {
                    var item = new Item();
                    item.ItemName = wikiItem.ItemName.Trim();
                    item.IconFileName = wikiItem.IconFileName.Trim();

                    foreach (string statLine in wikiItem.Stats)
                    {
                        var lineTokens = statLine
                            .Replace(")(", ") (") // make life easier when parsing Effect corner cases
                            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        parseStatLineTokens(lineTokens, item);
                    }

                    items.Add(item);
                }
            }

            // There will inevitably be a few dozen items on the wiki that are listed more than once.  99% of the time, it's
            // an item listed twice - once with the correct name capitalization, and once with a weird lowercase name.  So
            // our best-guess logic is that when there are duplicate wiki entries for an item, we'll use the one that
            // has the most capital letters in the item's name.  
            // (You can see this for yourself by running the List-Duplicate-Names command in TunnelQuest.DatabaseBuilder.)
            var itemsWithoutDuplicates = items
                .GroupBy(item => item.ItemName.ToLower())
                .Select(group => group.OrderBy(item => countUppercase(item.ItemName)).FirstOrDefault())
                .ToList();

            return itemsWithoutDuplicates;
        }

        private void parseStatLineTokens(string[] tokens, Item item)
        {
            int currentIndex = 0;

            while (currentIndex < tokens.Length)
            {
                switch (tokens[currentIndex].ToUpper().TrimEnd(':'))
                {
                    case "EFFECT":
                        currentIndex++;

                        var newItemEffect = new ItemEffect()
                        {
                            ItemName = item.ItemName
                        };

                        // build newItemEffect.EffectName
                        newItemEffect.EffectName = "";
                        while (currentIndex < tokens.Length && !tokens[currentIndex].StartsWith('('))
                        {
                            newItemEffect.EffectName += " " + tokens[currentIndex];
                            currentIndex++;
                        }
                        newItemEffect.EffectName = newItemEffect.EffectName.Trim();

                        if (currentIndex < tokens.Length)
                        {
                            // corner case: ignore the token "(spell)" if it's found
                            if (tokens[currentIndex].Equals("(spell)", StringComparison.InvariantCultureIgnoreCase) && tokens.Length > (currentIndex + 1))
                                currentIndex++;

                            // determine newItemEffect.EffectTypeCode
                            if (tokens[currentIndex].StartsWith("(any", StringComparison.InvariantCultureIgnoreCase)
                                || tokens[currentIndex].StartsWith("(inventory", StringComparison.InvariantCultureIgnoreCase))
                            {
                                newItemEffect.EffectTypeCode = "ClickAnySlot";
                            }
                            else if (tokens[currentIndex].StartsWith("(must", StringComparison.InvariantCultureIgnoreCase))
                            {
                                newItemEffect.EffectTypeCode = "ClickEquipped";
                            }
                            else if (tokens[currentIndex].StartsWith("(worn", StringComparison.InvariantCultureIgnoreCase))
                            {
                                newItemEffect.EffectTypeCode = "Worn";
                            }
                            else if (tokens[currentIndex].StartsWith("(combat", StringComparison.InvariantCultureIgnoreCase))
                            {
                                newItemEffect.EffectTypeCode = "Combat";
                            }
                            else if (tokens[currentIndex].Equals("(instant)", StringComparison.InvariantCultureIgnoreCase))
                            {
                                // corner case
                                // Line was in format "Effect: EffectName (Instant)"
                                newItemEffect.EffectTypeCode = "ClickEquipped"; // random guess
                                newItemEffect.CastingTime = 0;
                            }
                            else if (tokens[currentIndex].StartsWith("(casting", StringComparison.InvariantCultureIgnoreCase))
                            {
                                // corner case
                                // Line was in format "Effect: EffectName (Casting Time: whatever)", without the EffectType
                                newItemEffect.EffectTypeCode = "ClickEquipped"; // random guess
                            }
                            else
                                throw new UnknownStatTokenException(tokens[currentIndex]);

                            currentIndex++;
                        }
                        else
                        {
                            // corner case
                            // Line was in format "Effect: EffectName" with nothing else after the name.  Assume it's a worn effect.
                            newItemEffect.EffectTypeCode = "Worn";
                        }


                        // try to parse CastingTime and MinimumLevel, either of which may or may not be present
                        while (currentIndex < tokens.Length)
                        {
                            // set newItemEffect.CastingTime
                            if (tokens[currentIndex].StartsWith("time", StringComparison.InvariantCultureIgnoreCase) && tokens.Length > (currentIndex + 1))
                            {
                                currentIndex++;

                                string castingTimeString = tokens[currentIndex].TrimEnd(')');
                                if (castingTimeString.Equals("instant", StringComparison.InvariantCultureIgnoreCase))
                                    newItemEffect.CastingTime = 0;
                                else
                                    newItemEffect.CastingTime = float.Parse(castingTimeString);
                            }

                            // set newItemEffect.MinimumLevel
                            if (tokens[currentIndex].Equals("level", StringComparison.InvariantCultureIgnoreCase) && tokens.Length > (currentIndex + 1))
                            {
                                currentIndex++;
                                newItemEffect.MinimumLevel = int.Parse(tokens[currentIndex]);
                            }

                            currentIndex++;
                        }

                        item.ItemEffects = new List<ItemEffect>();
                        item.ItemEffects.Add(newItemEffect);

                        break;

                        // STUB uncomment these lines!
                        //default:
                        //  throw new Exception("Unrecognized stat token " + tokens[currentIndex]);
                }

                currentIndex++;
            } // end while (currentIndex < tokens.Length)
        }

        private int countUppercase(string input)
        {
            int count = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                    count++;
            }

            return count;
        }
    }
}


