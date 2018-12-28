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
                // insert static data
                insertClasses(context);
                insertRaces(context);
                insertSizes(context);
                insertSlots(context);
                insertEffectTypes(context);
                insertWeaponSkills(context);
                insertStats(context);
                insertDeities(context);


                // insert item and effect data scraped from the wiki

                var wikiData = WikiItemData.ReadFromEmbeddedResource();
                var items = parseWikiItems(wikiData);
                var effectNameNormalizer = getEffectNameNormalizer(items);

                insertEffects(context, effectNameNormalizer);

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

                // insert items
                context.AddRange(items);

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
                        // delete item and effect data scraped from the wiki

                        var wikiData = WikiItemData.ReadFromEmbeddedResource();
                        var items = parseWikiItems(wikiData);
                        var effectNameNormalizer = getEffectNameNormalizer(items);

                        deleteItems(context, items);
                        context.SaveChanges();
                        deleteEffects(context, effectNameNormalizer);

                        // delete static data
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
                    ClassCode = ClassCodes.Enchanter,
                    ClassName = ClassNames.Enchanter
                },
                new Class()
                {
                    ClassCode = ClassCodes.Magician,
                    ClassName = ClassNames.Magician
                },
                new Class()
                {
                    ClassCode = ClassCodes.Necromancer,
                    ClassName = ClassNames.Necromancer
                },
                new Class()
                {
                    ClassCode = ClassCodes.Wizard,
                    ClassName = ClassNames.Wizard
                },
                new Class()
                {
                    ClassCode = ClassCodes.Cleric,
                    ClassName = ClassNames.Cleric
                },
                new Class()
                {
                    ClassCode = ClassCodes.Druid,
                    ClassName = ClassNames.Druid
                },
                new Class()
                {
                    ClassCode = ClassCodes.Shaman,
                    ClassName = ClassNames.Shaman
                },
                new Class()
                {
                    ClassCode = ClassCodes.Bard,
                    ClassName = ClassNames.Bard
                },
                new Class()
                {
                    ClassCode = ClassCodes.Monk,
                    ClassName = ClassNames.Monk
                },
                new Class()
                {
                    ClassCode = ClassCodes.Ranger,
                    ClassName = ClassNames.Ranger
                },
                new Class()
                {
                    ClassCode = ClassCodes.Rogue,
                    ClassName = ClassNames.Rogue
                },
                new Class()
                {
                    ClassCode = ClassCodes.Paladin,
                    ClassName = ClassNames.Paladin
                },
                new Class()
                {
                    ClassCode = ClassCodes.ShadowKnight,
                    ClassName = ClassNames.ShadowKnight
                },
                new Class()
                {
                    ClassCode = ClassCodes.Warrior,
                    ClassName = ClassNames.Warrior
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
                    RaceCode = RaceCodes.Barbarian,
                    RaceName = RaceNames.Barbarian
                },
                new Race() {
                    RaceCode = RaceCodes.DarkElf,
                    RaceName = RaceNames.DarkElf
                },
                new Race() {
                    RaceCode = RaceCodes.Dwarf,
                    RaceName = RaceNames.Dwarf
                },
                new Race() {
                    RaceCode = RaceCodes.Erudite,
                    RaceName = RaceNames.Erudite
                },
                new Race() {
                    RaceCode = RaceCodes.Gnome,
                    RaceName = RaceNames.Gnome
                },
                new Race() {
                    RaceCode = RaceCodes.HalfElf,
                    RaceName = RaceNames.HalfElf
                },
                new Race() {
                    RaceCode = RaceCodes.Halfling,
                    RaceName = RaceNames.Halfling
                },
                new Race() {
                    RaceCode = RaceCodes.HighElf,
                    RaceName = RaceNames.HighElf
                },
                new Race() {
                    RaceCode = RaceCodes.Human,
                    RaceName = RaceNames.Human
                },
                new Race() {
                    RaceCode = RaceCodes.Iksar,
                    RaceName = RaceNames.Iksar
                },
                new Race() {
                    RaceCode = RaceCodes.Ogre,
                    RaceName = RaceNames.Ogre
                },
                new Race() {
                    RaceCode = RaceCodes.Troll,
                    RaceName = RaceNames.Troll
                },
                new Race() {
                    RaceCode = RaceCodes.WoodElf,
                    RaceName = RaceNames.WoodElf
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
                    SizeCode = SizeCodes.Tiny
                },
                new Size() {
                    SizeCode = SizeCodes.Small
                },
                new Size() {
                    SizeCode = SizeCodes.Medium
                },
                new Size() {
                    SizeCode = SizeCodes.Large
                },
                new Size() {
                    SizeCode = SizeCodes.Giant
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
                    SlotCode = SlotCodes.Arms
                },
                new Slot() {
                    SlotCode = SlotCodes.Back
                },
                new Slot() {
                    SlotCode = SlotCodes.Chest
                },
                new Slot() {
                    SlotCode = SlotCodes.Ear
                },
                new Slot() {
                    SlotCode = SlotCodes.Face
                },
                new Slot() {
                    SlotCode = SlotCodes.Feet
                },
                new Slot() {
                    SlotCode = SlotCodes.Finger
                },
                new Slot() {
                    SlotCode = SlotCodes.Hands
                },
                new Slot() {
                    SlotCode = SlotCodes.Head
                },
                new Slot() {
                    SlotCode = SlotCodes.Legs
                },
                new Slot() {
                    SlotCode = SlotCodes.Neck
                },
                new Slot() {
                    SlotCode = SlotCodes.Shoulders
                },
                new Slot() {
                    SlotCode = SlotCodes.Waist
                },
                new Slot() {
                    SlotCode = SlotCodes.Wrist
                },
                new Slot() {
                    SlotCode = SlotCodes.Primary
                },
                new Slot() {
                    SlotCode = SlotCodes.Secondary
                },
                new Slot() {
                    SlotCode = SlotCodes.Range
                },
                new Slot() {
                    SlotCode = SlotCodes.Ammo
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
                    EffectTypeCode = EffectTypeCodes.Combat
                },
                new EffectType() {
                    EffectTypeCode = EffectTypeCodes.Worn
                },
                new EffectType() {
                    EffectTypeCode = EffectTypeCodes.ClickAnySlot
                },
                new EffectType() {
                    EffectTypeCode = EffectTypeCodes.ClickEquipped
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
                    WeaponSkillCode = WeaponSkillCodes.OneHandBlunt
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.TwoHandBlunt
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.OneHandSlashing
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.TwoHandSlashing
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.OneHandPiercing
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.TwoHandPiercing
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.Archery
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.Throwing
                },
                new WeaponSkill() {
                    WeaponSkillCode = WeaponSkillCodes.HandToHand
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
                    StatCode = StatCodes.Strength
                },
                new Stat() {
                    StatCode = StatCodes.Stamina
                },
                new Stat() {
                    StatCode = StatCodes.Agility
                },
                new Stat() {
                    StatCode = StatCodes.Dexterity
                },
                new Stat() {
                    StatCode = StatCodes.Wisdom
                },
                new Stat() {
                    StatCode = StatCodes.Intelligence
                },
                new Stat() {
                    StatCode = StatCodes.Charisma
                },
                new Stat() {
                    StatCode = StatCodes.HitPoints
                },
                new Stat() {
                    StatCode = StatCodes.Mana
                },
                new Stat() {
                    StatCode = StatCodes.ArmorClass
                },
                new Stat() {
                    StatCode = StatCodes.MagicResist
                },
                new Stat() {
                    StatCode = StatCodes.PoisonResist
                },
                new Stat() {
                    StatCode = StatCodes.DiseaseResist
                },
                new Stat() {
                    StatCode = StatCodes.FireResist
                },
                new Stat() {
                    StatCode = StatCodes.ColdResist
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
                    DeityName = DeityNames.CazicThule
                },
                new Deity() {
                    DeityName = DeityNames.Tunare
                },
                new Deity() {
                    DeityName = DeityNames.Karana
                },
                new Deity() {
                    DeityName = DeityNames.BrellSerilis
                },
                new Deity() {
                    DeityName = DeityNames.Innoruuk
                },
                new Deity() {
                    DeityName = DeityNames.Quellious
                },
                new Deity() {
                    DeityName = DeityNames.Bertoxxulous
                },
                new Deity() {
                    DeityName = DeityNames.ErollisiMarr
                },
                new Deity() {
                    DeityName = DeityNames.Bristlebane
                },
                new Deity() {
                    DeityName = DeityNames.MithanielMarr
                },
                new Deity() {
                    DeityName = DeityNames.Prexus
                },
                new Deity() {
                    DeityName = DeityNames.RallosZek
                },
                new Deity() {
                    DeityName = DeityNames.RodcetNife
                },
                new Deity() {
                    DeityName = DeityNames.SolusekRo
                },
                new Deity() {
                    DeityName = DeityNames.TheTribunal
                }
            };
        }
        #endregion

        #region item
        private void deleteItems(TunnelQuestContext context, IEnumerable<Item> items)
        {
            // create a list of items with ONLY the names for use as deletion keys, and let the
            // cascading deletes on the foreign keys handle the deletions in the child tables
            var itemsToDelete = new List<Item>();
            foreach (var fullItem in items)
            {
                itemsToDelete.Add(new Item() { ItemName = fullItem.ItemName });
            }
            context.RemoveRange(itemsToDelete);
        }
        #endregion

        #region effect
        private void insertEffects(TunnelQuestContext context, Dictionary<string, string> effectNameNormalizer)
        {
            foreach (string effectName in effectNameNormalizer.Values)
            {
                context.Add(new Effect()
                {
                    EffectName = effectName
                });
            }
        }

        private void deleteEffects(TunnelQuestContext context, Dictionary<string, string> effectNameNormalizer)
        {
            foreach (string effectName in effectNameNormalizer.Values)
            {
                context.Remove(new Effect()
                {
                    EffectName = effectName
                });
            }
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
            // There will inevitably be a few dozen items on the wiki that are listed more than once.  99% of the time, it's
            // an item listed twice - once with the correct name capitalization, and once with a weird lowercase name.  So
            // our best-guess logic is that when there are duplicate wiki entries for an item, we'll use the one that
            // has the most capital letters in the item's name.  
            // (You can see this for yourself by running the List-Duplicate-Names command in TunnelQuest.DatabaseBuilder.)
            var wikiItemsWithoutDuplicates = wikiData
                .Where(item => item.Stats != null && !String.IsNullOrWhiteSpace(item.ItemName))
                .GroupBy(item => item.ItemName.ToLower())
                .Select(group => group.OrderBy(item => countUppercase(item.ItemName)).FirstOrDefault());

            var items = new List<Item>();
            var errorItems = new List<string>();
            foreach (WikiItemData wikiItem in wikiItemsWithoutDuplicates)
            {
                cleanWikiItem(wikiItem);

                try
                {
                    items.Add(parseWikiItem(wikiItem));
                }
                catch (Exception)
                {
                    errorItems.Add(wikiItem.ToString());
                }
            }

            if (errorItems.Count > 0)
                throw new Exception("Error occurred parsing the following items: " + String.Join(", ", errorItems));

            return items;
        }

        // If an item's wiki page is particularly messed up, I just hard-code the item here
        // rather than trying to make the parser handle every single janky wiki page.  It's
        // unlikely that the stats for any of these items are going to change.
        private void cleanWikiItem(WikiItemData wikiItem)
        {
            switch (wikiItem.ItemName.ToLower().Trim())
            {
                case "ice silk amice":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM QUEST ITEM",
                        "Slot: SHOULDERS",
                        "AC: 4",
                        "CHA: +5 INT: +3 HP: +5 MANA: +10 SV COLD: +10",
                        "WT: 0.3 Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk bracelet":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: WRIST",
                        "AC: 3",
                        "STA: +5  CHA: +3  HP: +5  MANA: +15 SV COLD: +5",
                        "WT: 0.3  Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk cap":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: HEAD",
                        "AC: 5",
                        "CHA: +11  WIS: +8  INT: +10  HP: +10  MANA: +20 SV COLD: +5  SV MAGIC: +5",
                        "WT: 0.3  Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk choker":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: NECK",
                        "AC: 4",
                        "CHA: +5  INT: +4  HP: +5  MANA: +10 SV COLD: +5  SV MAGIC: +5",
                        "WT: 0.2  Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk cloak":
                    wikiItem.Stats = new string[] {
                        "Slot: BACK",
                        "AC: 6",
                        "CHA: +6  INT: +8  HP: +5  MANA: +10 SV COLD: +10  SV MAGIC: +10",
                        "WT: 0.6  Size: MEDIUM",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk gloves":
                    wikiItem.Stats = new string[] {
                        "Slot: BACK",
                        "AC: 6",
                        "CHA: +6  INT: +8  HP: +5  MANA: +10 SV COLD: +10  SV MAGIC: +10",
                        "WT: 0.6  Size: MEDIUM",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk lined shoes":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: FEET",
                        "AC: 4",
                        "DEX: +5  CHA: +4  AGI: +4  HP: +20  MANA: +8 SV COLD: +10",
                        "WT: 0.7  Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk pantaloons":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: LEGS",
                        "AC: 5",
                        "CHA: +6  INT: +5  HP: +20  MANA: +5 SV COLD: +9",
                        "WT: 0.7  Size: MEDIUM",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk robe":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: CHEST",
                        "AC: 12",
                        "DEX: +11  CHA: +10  INT: +14  HP: +15  MANA: +25 SV COLD: +10",
                        "WT: 1.0  Size: MEDIUM",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk sash":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: WAIST",
                        "AC: 4",
                        "CHA: +1  WIS: +1  INT: +3  HP: +5  MANA: +10 SV COLD: +5  SV MAGIC: +5",
                        "WT: 0.2  Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk sleeves":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: ARMS",
                        "AC: 4",
                        "CHA: +1  INT: +8  HP: +10  MANA: +10 SV COLD: +9",
                        "WT: 0.4  Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;

                case "ice silk swatch":
                    wikiItem.Stats = new string[] {
                        "WT: 0.1  Size: SMALL",
                        "Class: ALL Race: ALL"
                    };
                    break;

                case "ice silk veil":
                    wikiItem.Stats = new string[] {
                        "MAGIC ITEM",
                        "Slot: FACE",
                        "AC: 3",
                        "CHA: +4  INT: +4  HP: +5  MANA: +10 SV COLD: +5  SV MAGIC: +5",
                        "WT: 0.2  Size: SMALL",
                        "Class: NEC WIZ MAG ENC Race: HUM ERU HIE DEF GNM IKS"
                    };
                    break;
            }
        }

        private Item parseWikiItem(WikiItemData wikiItem)
        {
            var item = new Item();
            item.ItemName = wikiItem.ItemName.Trim();
            item.IconFileName = wikiItem.IconFileName.Trim();

            foreach (string statLine in wikiItem.Stats)
            {
                // check for lines that we know to ignore completely
                if (statLine.StartsWith("This is a") || statLine == "Slot 1, Type 7 (General: Group)")
                    continue;

                var lineChunks = statLine
                    .Replace(")(", ") (") // make life easier when parsing Effect corner cases
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string firstChunk = cleanChunk(lineChunks[0]);

                // first check for tokens that we know will take up an entire line
                if (firstChunk == "AC")
                {
                    if (item.ItemStats.Where(itemStat => itemStat.StatCode == StatCodes.ArmorClass).Count() > 0)
                        throw new Exception("Multiple AC lines found for " + wikiItem.ToString());
                    else
                        item.ItemStats.Add(new ItemStat()
                        {
                            ItemName = item.ItemName,
                            Item = item,
                            StatCode = StatCodes.ArmorClass
                        });
                }
                else if (firstChunk == "DMG")
                {
                    if (item.AttackDamage != null)
                        throw new Exception("Multiple DMG lines found for " + wikiItem.ToString());
                    else
                        item.AttackDamage = Int32.Parse(cleanChunk(lineChunks[1]));
                }
                else if (firstChunk == "SKILL")
                {
                    if (item.WeaponSkillCode != null)
                        throw new Exception("Multiple Skill lines found for " + wikiItem.ToString());
                    else
                        parseWeaponSkillLine(lineChunks, item);
                }
                else if (firstChunk == "SLOT")
                {
                    if (item.ItemSlots.Count > 0)
                        throw new Exception("Multiple Slot lines found for " + wikiItem.ToString());
                    else
                        parseSlotsLine(lineChunks, item);
                }
                else if (firstChunk == "WT")
                {
                    if (item.Weight != 0)
                        throw new Exception("Multiple WT/Size lines found for " + wikiItem.ToString());
                    else
                    {
                        item.Weight = float.Parse(cleanChunk(lineChunks[1]));
                        item.SizeCode = SizeCodes.All.Where(code => code.Equals(cleanChunk(lineChunks[3]), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                        if (String.IsNullOrWhiteSpace(item.SizeCode))
                            throw new Exception("Unrecognized Size");
                    }
                }
                else if (firstChunk == "CLASS")
                {
                    if (item.ItemClasses.Count > 0)
                        throw new Exception("Multiple Class lines found for " + wikiItem.ToString());
                    else
                        parseClassLine(lineChunks, item);
                }
                else if (firstChunk == "RACE")
                {
                    if (item.ItemRaces.Count > 0)
                        throw new Exception("Multiple Race lines found for " + wikiItem.ToString());
                    else
                        parseRaceLine(lineChunks, item);
                }
                else if (firstChunk == "DEITY")
                {
                    if (item.Deities.Count > 0)
                        throw new Exception("Multiple Deity lines found for " + wikiItem.ToString());
                    else
                        parseDeityLine(lineChunks, item);
                }
                else if (firstChunk == "EFFECT")
                {
                    // there CAN be multiple effect lines in very rare cases
                    parseEffectLine(lineChunks, item);
                }
                else
                {
                    // it wasn't a whole-line token, so next check for tokens that can share a line

                    int currentIndex = 0;
                    while (currentIndex < lineChunks.Length)
                    {
                        string currentChunk = cleanChunk(lineChunks[currentIndex]);
                        bool isChunkHandled = false;

                        if (currentChunk == "NODROP")
                        {
                            item.IsNoDrop = true;
                            isChunkHandled = true;
                        }
                        else if (currentChunk == "TEMPORARY")
                        {
                            item.IsTemporary = true;
                            isChunkHandled = true;
                        }
                        else if (currentChunk == "EXPENDABLE")
                        {
                            item.IsExpendable = true;
                            isChunkHandled = true;
                        }
                        else if (currentIndex + 1 < lineChunks.Length)
                        {
                            // this is NOT the last token in the line, so check for two-token combos

                            string nextChunk = cleanChunk(lineChunks[currentIndex + 1]);
                            if (currentChunk == "MAGIC" && nextChunk == "ITEM")
                            {
                                item.IsMagic = true;
                                currentIndex++;
                                isChunkHandled = true;
                            }
                            else if (currentChunk == "LORE" && nextChunk == "ITEM")
                            {
                                item.IsLore = true;
                                currentIndex++;
                                isChunkHandled = true;
                            }
                            else if (currentChunk == "QUEST" && nextChunk == "ITEM")
                            {
                                item.IsQuestItem = true;
                                currentIndex++;
                                isChunkHandled = true;
                            }
                            else if (currentChunk == "NO" && nextChunk == "DROP")
                            {
                                item.IsNoDrop = true;
                                currentIndex++;
                                isChunkHandled = true;
                            }
                            else if (currentChunk == "CHARGES")
                            {
                                if (nextChunk == "UNLIMITED")
                                    item.MaxCharges = null;
                                else
                                    item.MaxCharges = Int32.Parse(nextChunk);
                                currentIndex++;
                                isChunkHandled = true;
                            }
                        }

                        // stub uncomment this
                        //if (!chunkHandled)
                        //    throw new Exception("Unrecognized raw stat token " + currentChunk);

                        currentIndex++;
                    } // end while (currentIndex < lineChunks.Length)
                } // end else
            }// end foreach (statLine)
            
            return item;
        }
        
        private string cleanChunk(string chunk)
        {
            return chunk.ToUpper().TrimEnd(':');
        }

        private void parseDeityLine(string[] lineChunks, Item item)
        {
            // stub!
        }

        private void parseRaceLine(string[] lineChunks, Item item)
        {
            // stub!
        }

        private void parseClassLine(string[] lineChunks, Item item)
        {
            string[] wikiCodes = lineChunks.Skip(1).ToArray();
            IEnumerable<string> confirmedCodes;

            if (wikiCodes[0].Equals("NONE", StringComparison.InvariantCultureIgnoreCase))
            {
                confirmedCodes = new string[] { };
            }
            else if (wikiCodes[0].Equals("ALL", StringComparison.InvariantCultureIgnoreCase))
            {
                confirmedCodes = ClassCodes.All;

                if (wikiCodes.Length > 1)
                {
                    if (wikiCodes[1].Equals("EXCEPT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var exceptCodes = wikiCodes.Skip(2);

                        var confirmedExceptCodes = ClassCodes.All.Where(cleanCode => exceptCodes.Contains(cleanCode, StringComparer.InvariantCultureIgnoreCase));
                        if (exceptCodes.Count() != confirmedExceptCodes.Count())
                            throw new Exception("Unrecognized classes for " + item.ItemName);

                        confirmedCodes = confirmedCodes.Where(code => !confirmedExceptCodes.Contains(code, StringComparer.InvariantCultureIgnoreCase));
                    }
                    else
                        throw new Exception("Couldn't parse Class line for " + item.ItemName);
                }
            }
            else
            {
                confirmedCodes = ClassCodes.All.Where(cleanCode => wikiCodes.Contains(cleanCode, StringComparer.InvariantCultureIgnoreCase));

                if (wikiCodes.Count() != confirmedCodes.Count())
                    throw new Exception("Unrecognized Classes in " + item.ItemName);
            }

            foreach (string code in confirmedCodes)
            {
                item.ItemClasses.Add(new ItemClass()
                {
                    ItemName = item.ItemName,
                    Item = item,
                    ClassCode = code
                });
            }
        }

        private void parseWeaponSkillLine(string[] lineChunks, Item item)
        {
            // parse weaponSkillCode
            string weaponSkillCode = "";
            for (int i = 1; i < lineChunks.Length; i++)
            {
                if (lineChunks[i].ToUpper() == "ATK")
                    break;
                else
                    weaponSkillCode += lineChunks[i] + " ";
            }
            weaponSkillCode = weaponSkillCode.TrimEnd();

            var cleanCode = WeaponSkillCodes.All.Where(code => code.Equals(weaponSkillCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (cleanCode == null)
                throw new Exception("Unrecognized weapon skill in " + item.ItemName);

            item.WeaponSkillCode = cleanCode;
            item.AttackDelay = Int32.Parse(lineChunks.Last());
        }

        private void parseSlotsLine(string[] lineChunks, Item item)
        {
            var wikiSlotCodes = lineChunks.Skip(1).ToArray();

            // clean up wiki data
            for (int i = 0; i < wikiSlotCodes.Length; i++)
            {
                if (wikiSlotCodes[i].Equals("FINGERS", StringComparison.InvariantCultureIgnoreCase))
                    wikiSlotCodes[i] = "FINGER";
                else if (wikiSlotCodes[i].Equals("SHOULDER", StringComparison.InvariantCultureIgnoreCase))
                    wikiSlotCodes[i] = "SHOULDERS";
            }

            var confirmedSlotCodes = SlotCodes.All.Where(cleanCode => wikiSlotCodes.Contains(cleanCode, StringComparer.InvariantCultureIgnoreCase));

            if (confirmedSlotCodes.Count() != wikiSlotCodes.Count())
                throw new Exception("Unrecognized slots in " + item.ItemName);

            foreach (string slotCode in confirmedSlotCodes)
            {
                item.ItemSlots.Add(new ItemSlot()
                {
                    ItemName = item.ItemName,
                    Item = item,
                    SlotCode = slotCode
                });
            }
        }

        private void parseEffectLine(string[] lineChunks, Item item)
        {
            var newItemEffect = new ItemEffect()
            {
                ItemName = item.ItemName
            };

            int currentIndex = 1;

            // build newItemEffect.EffectName
            newItemEffect.EffectName = "";
            while (currentIndex < lineChunks.Length && !lineChunks[currentIndex].StartsWith('('))
            {
                newItemEffect.EffectName += " " + lineChunks[currentIndex];
                currentIndex++;
            }
            newItemEffect.EffectName = newItemEffect.EffectName.Trim();

            if (currentIndex < lineChunks.Length)
            {
                // corner case: ignore the token "(spell)" if it's found
                if (lineChunks[currentIndex].Equals("(spell)", StringComparison.InvariantCultureIgnoreCase) && lineChunks.Length > (currentIndex + 1))
                    currentIndex++;

                // determine newItemEffect.EffectTypeCode
                if (lineChunks[currentIndex].StartsWith("(any", StringComparison.InvariantCultureIgnoreCase)
                    || lineChunks[currentIndex].StartsWith("(inventory", StringComparison.InvariantCultureIgnoreCase))
                {
                    newItemEffect.EffectTypeCode = "ClickAnySlot";
                }
                else if (lineChunks[currentIndex].StartsWith("(must", StringComparison.InvariantCultureIgnoreCase))
                {
                    newItemEffect.EffectTypeCode = "ClickEquipped";
                }
                else if (lineChunks[currentIndex].StartsWith("(worn", StringComparison.InvariantCultureIgnoreCase))
                {
                    newItemEffect.EffectTypeCode = "Worn";
                }
                else if (lineChunks[currentIndex].StartsWith("(combat", StringComparison.InvariantCultureIgnoreCase))
                {
                    newItemEffect.EffectTypeCode = "Combat";
                }
                else if (lineChunks[currentIndex].Equals("(instant)", StringComparison.InvariantCultureIgnoreCase))
                {
                    // corner case
                    // Line was in format "Effect: EffectName (Instant)"
                    newItemEffect.EffectTypeCode = "ClickEquipped"; // random guess
                    newItemEffect.CastingTime = 0;
                }
                else if (lineChunks[currentIndex].StartsWith("(casting", StringComparison.InvariantCultureIgnoreCase))
                {
                    // corner case
                    // Line was in format "Effect: EffectName (Casting Time: whatever)", without the EffectType
                    newItemEffect.EffectTypeCode = "ClickEquipped"; // random guess
                }
                else
                    throw new UnknownStatTokenException(lineChunks[currentIndex]);

                currentIndex++;
            }
            else
            {
                // corner case
                // Line was in format "Effect: EffectName" with nothing else after the name.  Assume it's a worn effect.
                newItemEffect.EffectTypeCode = "Worn";
            }


            // try to parse CastingTime and MinimumLevel, either of which may or may not be present
            while (currentIndex < lineChunks.Length)
            {
                // set newItemEffect.CastingTime
                if (lineChunks[currentIndex].StartsWith("time", StringComparison.InvariantCultureIgnoreCase) && lineChunks.Length > (currentIndex + 1))
                {
                    currentIndex++;

                    string castingTimeString = lineChunks[currentIndex].TrimEnd(')');
                    if (castingTimeString.Equals("instant", StringComparison.InvariantCultureIgnoreCase))
                        newItemEffect.CastingTime = 0;
                    else
                        newItemEffect.CastingTime = float.Parse(castingTimeString);
                }

                // set newItemEffect.MinimumLevel
                if (lineChunks[currentIndex].Equals("level", StringComparison.InvariantCultureIgnoreCase) && lineChunks.Length > (currentIndex + 1))
                {
                    currentIndex++;
                    newItemEffect.MinimumLevel = int.Parse(lineChunks[currentIndex]);
                }

                currentIndex++;
            }

            item.ItemEffects.Add(newItemEffect);
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


