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
    public partial class InsertItemAndEffectData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = new TunnelQuestContext())
            {
                // insert item and effect data scraped from the wiki

                var wikiData = WikiItemData.ReadFromEmbeddedResource();
                var items = parseWikiItems(wikiData);
                var effectNameNormalizer = getEffectNameNormalizer(items);

                insertEffects(context, effectNameNormalizer);

                // use effectNameNormalizer to make sure all items use the most popular spelling of their effect names
                foreach (var item in items.Where(item => item.EffectName != null))
                {
                    item.EffectName = effectNameNormalizer[item.EffectName.ToLower()];
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
                .Where(item => !String.IsNullOrWhiteSpace(item.EffectName))
                .Select(item => item.EffectName)
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
                try
                {
                    items.Add(parseWikiItem(wikiItem));
                }
                catch (Exception ex)
                {
                    errorItems.Add(wikiItem.ToString());
                }
            }

            if (errorItems.Count > 0)
                throw new Exception(errorItems.Count.ToString() + " errors occurred while parsing items: " + String.Join(", ", errorItems));

            return items;
        }
        
        private Item parseWikiItem(WikiItemData wikiItem)
        {
            var item = new Item();
            item.ItemName = wikiItem.ItemName.Trim();
            item.IconFileName = wikiItem.IconFileName.Trim();

            foreach (string statLine in wikiItem.Stats)
            {
                try
                {
                    parseStatLine(wikiItem, item, statLine);
                }
                catch (UnknownStatTokenException)
                {
                    // add any unparseable lines as InfoText
                    item.Info.Add(new ItemInfoLine()
                    {
                        ItemName = item.ItemName,
                        Item = item,
                        Text = statLine
                    });
                }
            }

            return item;
        }

        private void parseStatLine(WikiItemData wikiItem, Item item, string statLine)
        {
            var lineChunks = statLine
                    .Replace(")(", ") (") // make life easier when parsing Effect corner cases
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(chunk => chunk.Trim())
                    .Where(chunk => chunk != ":" && chunk != "+")
                    .ToArray();

            string firstChunk = cleanChunk(lineChunks[0]);
            string secondChunk = (lineChunks.Length == 1 ? null : cleanChunk(lineChunks[1]));

            // first check for tokens that we know will take up an entire line
            if (firstChunk == "AC")
            {
                if (item.ArmorClass != null)
                    throw new Exception("Multiple AC lines found for " + wikiItem.ToString());
                item.ArmorClass = Int32.Parse(secondChunk);
            }
            else if (firstChunk == "DMG")
            {
                if (item.AttackDamage != null)
                    throw new Exception("Multiple DMG lines found for " + wikiItem.ToString());
                item.AttackDamage = Int32.Parse(secondChunk);
            }
            else if (firstChunk == "SKILL" && secondChunk != "MOD")
            {
                if (item.WeaponSkillCode != null)
                    throw new Exception("Multiple Skill lines found for " + wikiItem.ToString());
                parseWeaponSkillLine(lineChunks, item);
            }
            else if (firstChunk == "SLOT" && secondChunk != "1")
            {
                if (item.Slots.Count > 0)
                    throw new Exception("Multiple Slot lines found for " + wikiItem.ToString());
                parseSlotsLine(lineChunks, item);
            }
            else if (firstChunk == "CLASS")
            {
                if (item.Classes.Count > 0)
                    throw new Exception("Multiple Class lines found for " + wikiItem.ToString());
                parseClassLine(lineChunks, item);
            }
            else if (firstChunk == "RACE")
            {
                if (item.Races.Count > 0)
                    throw new Exception("Multiple Race lines found for " + wikiItem.ToString());
                parseRaceLine(lineChunks, item);
            }
            else if (firstChunk == "DEITY" || firstChunk == "DIETY")
            {
                if (item.Deities.Count > 0)
                    throw new Exception("Multiple Deity lines found for " + wikiItem.ToString());
                parseDeityLine(lineChunks, item);
            }
            else if (firstChunk == "EFFECT")
            {
                if (item.EffectName != null)
                    throw new Exception("Multiple Effect values found for " + wikiItem.ToString());
                parseEffectLine(lineChunks, item);
            }
            else if (firstChunk == "REQUIRED")
            {
                if (item.RequiredLevel != null)
                    throw new Exception("Multiple Required Level values for " + wikiItem.ToString());
                item.RequiredLevel = Int32.Parse(lineChunks.Last().TrimEnd('.'));
            }
            else if (firstChunk == "SINGING")
            {
                if (item.SingingModifier != null)
                    throw new Exception("Multiple Singing lines found for " + wikiItem.ToString());

                if (lineChunks.Length < 3)
                    item.SingingModifier = 0;
                else
                    item.SingingModifier = Convert.ToInt32(cleanChunk(lineChunks[2]));
            }
            else if (firstChunk == "PERCUSSION")
            {
                if (item.PercussionModifier != null)
                    throw new Exception("Multiple Percussion lines found for " + wikiItem.ToString());

                if (lineChunks.Length < 3)
                    item.PercussionModifier = 0;
                else
                    item.PercussionModifier = Convert.ToInt32(cleanChunk(lineChunks[2]));
            }
            else if (firstChunk == "STRINGED")
            {
                if (item.StringedModifier != null)
                    throw new Exception("Multiple Stringed lines found for " + wikiItem.ToString());

                if (lineChunks.Length < 3)
                    item.StringedModifier = 0;
                else
                    item.StringedModifier = Convert.ToInt32(cleanChunk(lineChunks[2]));
            }
            else if (firstChunk == "BRASS")
            {
                if (item.BrassModifier != null)
                    throw new Exception("Multiple Brass lines found for " + wikiItem.ToString());

                if (lineChunks.Length < 3)
                    item.BrassModifier = 0;
                else
                    item.BrassModifier = Convert.ToInt32(cleanChunk(lineChunks[2]));
            }
            else if (firstChunk == "WIND")
            {
                if (item.WindModifier != null)
                    throw new Exception("Multiple Wind lines found for " + wikiItem.ToString());

                if (lineChunks.Length < 3)
                    item.WindModifier = 0;
                else
                    item.WindModifier = Convert.ToInt32(cleanChunk(lineChunks[2]));
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
                    else if (currentChunk == "TEMPORARY" || currentChunk == "NORENT")
                    {
                        item.IsTemporary = true;
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "EXPENDABLE")
                    {
                        item.IsExpendable = true;
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "ARTIFACT")
                    {
                        item.IsArtifact = true;
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "WT")
                    {
                        if (item.Weight != 0)
                            throw new Exception("Multiple WT values found for " + wikiItem.ToString());

                        currentIndex++;
                        string nextChunk = cleanChunk(lineChunks[currentIndex]);


                        item.Weight = float.Parse(nextChunk);
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "SIZE")
                    {
                        currentIndex++;
                        string nextChunk = cleanChunk(lineChunks[currentIndex]);

                        if (nextChunk == "CAPACITY")
                        {
                            currentIndex++;
                            var nextNextChunk = cleanChunk(lineChunks[currentIndex]);

                            if (item.CapacitySizeCode != null)
                                throw new Exception("Multiple Size Capacity values found for " + wikiItem.ToString());

                            item.CapacitySizeCode = SizeCodes.All.Where(code => code.Equals(nextNextChunk, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                            if (String.IsNullOrWhiteSpace(item.CapacitySizeCode))
                                throw new Exception("Unrecognized Size Capacity" + nextNextChunk);
                        }
                        else
                        {
                            if (item.SizeCode != null)
                                throw new Exception("Multiple Size values found for " + wikiItem.ToString());

                            item.SizeCode = SizeCodes.All.Where(code => code.Equals(nextChunk, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                            if (String.IsNullOrWhiteSpace(item.SizeCode))
                                throw new Exception("Unrecognized Size " + nextChunk);
                        }

                        isChunkHandled = true;
                    }
                    else if (currentChunk == "CAPACITY")
                    {
                        if (item.Capacity != null)
                            throw new Exception("Multiple Capacity values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Capacity = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "RANGE")
                    {
                        if (item.Range != null)
                            throw new Exception("Multiple Range values found for " + wikiItem.ToString());

                        currentIndex++;
                        string rangeString = cleanChunk(lineChunks[currentIndex]);
                        if (rangeString.Contains('/') || (lineChunks.Length > currentIndex + 1 && lineChunks[currentIndex + 1].Contains('/')))
                        {
                            // there are multiple possible ranges (i.e. crafted arrows), so just set Range = null
                            item.Range = null;

                            while (cleanChunk(lineChunks[currentIndex + 1]) != "SIZE")
                            {
                                currentIndex++;
                            }
                        }
                        else
                            item.Range = Int32.Parse(rangeString);
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "HASTE")
                    {
                        if (item.Haste != null)
                            throw new Exception("Multiple Haste lines found for " + wikiItem.ToString());

                        currentIndex++;
                        item.Haste = float.Parse("0." + cleanChunk(lineChunks[currentIndex]).TrimStart('+').TrimEnd('%'));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "STR")
                    {
                        if (item.Strength != null)
                            throw new Exception("Multiple STR values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Strength = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "STA")
                    {
                        if (item.Stamina != null)
                            throw new Exception("Multiple STA values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Stamina = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "AGI")
                    {
                        if (item.Agility != null)
                            throw new Exception("Multiple AGI values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Agility = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "DEX")
                    {
                        if (item.Dexterity != null)
                            throw new Exception("Multiple DEX values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Dexterity = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "WIS")
                    {
                        if (item.Wisdom != null)
                            throw new Exception("Multiple WIS values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Wisdom = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "INT")
                    {
                        if (item.Intelligence != null)
                            throw new Exception("Multiple INT values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Intelligence = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "CHA")
                    {
                        if (item.Charisma != null)
                            throw new Exception("Multiple CHA values for " + wikiItem.ToString());

                        currentIndex++;
                        item.Charisma = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentChunk == "HP")
                    {
                        currentIndex++;
                        var nextChunk = cleanChunk(lineChunks[currentIndex]);

                        int hpValue;
                        if (Int32.TryParse(nextChunk, out hpValue))
                        {
                            if (item.HitPoints != null)
                                throw new Exception("Multiple HP values for " + wikiItem.ToString());

                            item.HitPoints = hpValue;
                            isChunkHandled = true;
                        }
                    }
                    else if (currentChunk == "MANA")
                    {
                        currentIndex++;
                        string nextChunk = cleanChunk(lineChunks[currentIndex]);

                        int manaValue;
                        if (Int32.TryParse(nextChunk, out manaValue))
                        {
                            if (item.Mana != null)
                                throw new Exception("Multiple MANA values for " + wikiItem.ToString());

                            isChunkHandled = true;
                        }
                        
                    }
                    else if (currentChunk == "AC")
                    {
                        if (item.ArmorClass != null)
                            throw new Exception("Multiple AC values for " + wikiItem.ToString());

                        currentIndex++;
                        item.ArmorClass = Int32.Parse(cleanChunk(lineChunks[currentIndex]));
                        isChunkHandled = true;
                    }
                    else if (currentIndex + 1 < lineChunks.Length)
                    {
                        // this is NOT the last token in the line, so check for two-token combos

                        string nextChunk = cleanChunk(lineChunks[currentIndex + 1]);

                        // for stats that are two tokens ("SV MAGIC", "SV POISON", etc)
                        if (currentChunk == "SV")
                        {
                            currentIndex += 2;
                            int value = Int32.Parse(cleanChunk(lineChunks[currentIndex]));

                            if (nextChunk == "MAGIC")
                            {
                                if (item.MagicResist != null)
                                    throw new Exception("Multiple SV MAGIC values found for " + wikiItem.ToString());
                                item.MagicResist = value;
                            }
                            else if (nextChunk == "POISON")
                            {
                                if (item.PoisonResist != null)
                                    throw new Exception("Multiple SV POISON values found for " + wikiItem.ToString());
                                item.PoisonResist = value;
                            }
                            else if (nextChunk == "DISEASE")
                            {
                                if (item.DiseaseResist != null)
                                    throw new Exception("Multiple SV DISEASE values found for " + wikiItem.ToString());
                                item.DiseaseResist = value;
                            }
                            else if (nextChunk == "FIRE")
                            {
                                if (item.FireResist != null)
                                    throw new Exception("Multiple SV FIRE values found for " + wikiItem.ToString());
                                item.FireResist = value;
                            }
                            else if (nextChunk == "COLD")
                            {
                                if (item.ColdResist != null)
                                    throw new Exception("Multiple SV COLD values found for " + wikiItem.ToString());
                                item.ColdResist = value;
                            }
                            else
                                throw new Exception("Unrecognized resist SV " + nextChunk);

                            isChunkHandled = true;
                        }
                        else if (currentChunk == "MAGIC" && nextChunk == "ITEM")
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
                        else if (currentChunk == "NO" && nextChunk == "RENT")
                        {
                            item.IsTemporary = true;
                            currentIndex++;
                            isChunkHandled = true;
                        }
                        else if (currentChunk == "NO" && nextChunk == "TRADE")
                        {
                            item.IsNoTrade = true;
                            currentIndex++;
                            isChunkHandled = true;
                        }
                        else if (currentChunk == "WEIGHT" && nextChunk == "REDUCTION")
                        {
                            if (item.WeightReduction != null)
                                throw new Exception("Multiple Weight Reduction values found for " + wikiItem.ToString());

                            isChunkHandled = true;
                            currentIndex += 2;
                            var nextNextChunk = cleanChunk(lineChunks[currentIndex]);

                            item.WeightReduction = float.Parse("0." + nextNextChunk.TrimEnd('%'));
                        }
                        else if (currentChunk == "CHARGES")
                        {
                            if (item.MaxCharges != null)
                                throw new Exception("Multiple Charges values found for " + wikiItem.ToString());

                            currentIndex++;
                            isChunkHandled = true;

                            if (nextChunk == "UNLIMITED")
                                item.MaxCharges = null;
                            else
                                item.MaxCharges = Int32.Parse(nextChunk);
                        }
                    }

                    if (!isChunkHandled)
                        throw new UnknownStatTokenException(currentChunk);

                    currentIndex++;
                } // end while (currentIndex < lineChunks.Length)
            } // end else
        }

        private string cleanChunk(string chunk)
        {
            return chunk.ToUpper().TrimEnd(':').TrimEnd(',');
        }

        private void parseDeityLine(string[] lineChunks, Item item)
        {
            string[] wikiNames = String.Join(' ', lineChunks.Skip(1).ToArray()).Split('/', StringSplitOptions.RemoveEmptyEntries);

            // correct common spelling errors
            for (int i = 0; i < wikiNames.Length; i++)
            {
                wikiNames[i] = wikiNames[i].Trim();

                if (wikiNames[i].Equals("Cazic Thule", StringComparison.InvariantCultureIgnoreCase))
                    wikiNames[i] = DeityNames.CazicThule;
            }

            IEnumerable<string> confirmedNames = DeityNames.All.Where(cleanName => wikiNames.Contains(cleanName, StringComparer.InvariantCultureIgnoreCase));

            if (wikiNames.Count() != confirmedNames.Count())
                throw new Exception("Unrecognized Deity in " + item.ItemName);

            foreach (string name in confirmedNames)
            {
                item.Deities.Add(new ItemDeity()
                {
                    ItemName = item.ItemName,
                    Item = item,
                    DeityName = name
                });
            }
        }

        private void parseRaceLine(string[] lineChunks, Item item)
        {
            string[] wikiCodes = lineChunks.Skip(1).Where(code => code.Equals("VAH", StringComparison.InvariantCultureIgnoreCase) == false && code.Equals("FRG", StringComparison.InvariantCultureIgnoreCase) == false).ToArray(); // filter out cats and frogs
            IEnumerable<string> confirmedCodes;

            if (wikiCodes[0].Equals("NONE", StringComparison.InvariantCultureIgnoreCase))
            {
                confirmedCodes = new string[] { };
            }
            else if (wikiCodes[0].Equals("ALL", StringComparison.InvariantCultureIgnoreCase) || wikiCodes[0].Equals("AL", StringComparison.InvariantCultureIgnoreCase) || wikiCodes[0].Equals("ANY", StringComparison.InvariantCultureIgnoreCase))
            {
                confirmedCodes = RaceCodes.All;

                if (wikiCodes.Length > 1)
                {
                    if (wikiCodes[1].Equals("EXCEPT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var exceptCodes = wikiCodes.Skip(2);

                        var confirmedExceptCodes = RaceCodes.All.Where(cleanCode => exceptCodes.Contains(cleanCode, StringComparer.InvariantCultureIgnoreCase));
                        if (exceptCodes.Count() != confirmedExceptCodes.Count())
                            throw new Exception("Unrecognized races for " + item.ItemName);

                        confirmedCodes = confirmedCodes.Where(code => !confirmedExceptCodes.Contains(code, StringComparer.InvariantCultureIgnoreCase));
                    }
                    else
                        throw new Exception("Couldn't parse Race line for " + item.ItemName);
                }
            }
            else
            {
                confirmedCodes = RaceCodes.All.Where(cleanCode => wikiCodes.Contains(cleanCode, StringComparer.InvariantCultureIgnoreCase));

                if (wikiCodes.Count() != confirmedCodes.Count())
                    throw new Exception("Unrecognized Races in " + item.ItemName);
            }

            foreach (string code in confirmedCodes)
            {
                item.Races.Add(new ItemRace()
                {
                    ItemName = item.ItemName,
                    Item = item,
                    RaceCode = code
                });
            }
        }

        private void parseClassLine(string[] lineChunks, Item item)
        {
            string[] wikiCodes = lineChunks.Skip(1).Where(code => !code.Equals("BST", StringComparison.InvariantCultureIgnoreCase)).ToArray(); // filter out Beastlords
            IEnumerable<string> confirmedCodes;

            if (wikiCodes[0].Equals("NONE", StringComparison.InvariantCultureIgnoreCase))
            {
                confirmedCodes = new string[] { };
            }
            else if (wikiCodes[0].Equals("ALL", StringComparison.InvariantCultureIgnoreCase) || wikiCodes[0].Equals("AL", StringComparison.InvariantCultureIgnoreCase) || wikiCodes[0].Equals("ANY", StringComparison.InvariantCultureIgnoreCase))
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
                item.Classes.Add(new ItemClass()
                {
                    ItemName = item.ItemName,
                    Item = item,
                    ClassCode = code
                });
            }
        }

        private void parseWeaponSkillLine(string[] lineChunks, Item item)
        {
            // parse skillCode
            string skillCode = "";
            for (int i = 1; i < lineChunks.Length; i++)
            {
                if (lineChunks[i].ToUpper() == "ATK")
                    break;
                else
                    skillCode += lineChunks[i] + " ";
            }
            skillCode = skillCode.TrimEnd();

            // normalize the wiki's throwing skill codes
            if (skillCode.Equals("Throwingv1", StringComparison.InvariantCultureIgnoreCase) || skillCode.Equals("Throwingv2", StringComparison.InvariantCultureIgnoreCase))
                skillCode = WeaponSkillCodes.Throwing;

            // normalize other common typos in the wiki stats
            else if (skillCode.Equals("1H Slash", StringComparison.InvariantCultureIgnoreCase))
                skillCode = WeaponSkillCodes.OneHandSlashing;
            else if (skillCode.Equals("2H Slash", StringComparison.InvariantCultureIgnoreCase))
                skillCode = WeaponSkillCodes.TwoHandSlashing;
            else if (skillCode.Equals("1HB", StringComparison.InvariantCultureIgnoreCase))
                skillCode = WeaponSkillCodes.OneHandBlunt;

            var weaponSkillCode = WeaponSkillCodes.All.Where(code => code.Equals(skillCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (weaponSkillCode == null)
                throw new UnknownStatTokenException(skillCode);

            item.WeaponSkillCode = weaponSkillCode;
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
                item.Slots.Add(new ItemSlot()
                {
                    ItemName = item.ItemName,
                    Item = item,
                    SlotCode = slotCode
                });
            }
        }

        private void parseEffectLine(string[] lineChunks, Item item)
        {
            int currentIndex = 1;

            // build item.EffectName
            item.EffectName = "";
            while (currentIndex < lineChunks.Length && !lineChunks[currentIndex].StartsWith('('))
            {
                item.EffectName += " " + lineChunks[currentIndex];
                currentIndex++;
            }
            item.EffectName = item.EffectName.Trim();

            if (currentIndex < lineChunks.Length)
            {
                // corner case: ignore the token "(spell)" if it's found
                if (lineChunks[currentIndex].Equals("(spell)", StringComparison.InvariantCultureIgnoreCase) && lineChunks.Length > (currentIndex + 1))
                    currentIndex++;

                // determine item.EffectTypeCode
                if (lineChunks[currentIndex].StartsWith("(any", StringComparison.InvariantCultureIgnoreCase)
                    || lineChunks[currentIndex].StartsWith("(inventory", StringComparison.InvariantCultureIgnoreCase))
                {
                    item.EffectTypeCode = "ClickAnySlot";
                }
                else if (lineChunks[currentIndex].StartsWith("(must", StringComparison.InvariantCultureIgnoreCase))
                {
                    item.EffectTypeCode = "ClickEquipped";
                }
                else if (lineChunks[currentIndex].StartsWith("(worn", StringComparison.InvariantCultureIgnoreCase))
                {
                    item.EffectTypeCode = "Worn";
                }
                else if (lineChunks[currentIndex].StartsWith("(combat", StringComparison.InvariantCultureIgnoreCase))
                {
                    item.EffectTypeCode = "Combat";
                }
                else if (lineChunks[currentIndex].Equals("(instant)", StringComparison.InvariantCultureIgnoreCase))
                {
                    // corner case
                    // Line was in format "Effect: EffectName (Instant)"
                    item.EffectTypeCode = "ClickEquipped"; // random guess
                    item.EffectCastingTime = 0;
                }
                else if (lineChunks[currentIndex].StartsWith("(casting", StringComparison.InvariantCultureIgnoreCase))
                {
                    // corner case
                    // Line was in format "Effect: EffectName (Casting Time: whatever)", without the EffectType
                    item.EffectTypeCode = "ClickEquipped"; // random guess
                }
                else
                    throw new UnknownStatTokenException(lineChunks[currentIndex]);

                currentIndex++;
            }
            else
            {
                // corner case
                // Line was in format "Effect: EffectName" with nothing else after the name.  Assume it's a worn effect.
                item.EffectTypeCode = "Worn";
            }


            // try to parse CastingTime and MinimumLevel, either of which may or may not be present
            while (currentIndex < lineChunks.Length)
            {
                // set item.EffectCastingTime
                if (lineChunks[currentIndex].StartsWith("time", StringComparison.InvariantCultureIgnoreCase) && lineChunks.Length > (currentIndex + 1))
                {
                    currentIndex++;

                    string castingTimeString = lineChunks[currentIndex].TrimEnd(')');
                    if (castingTimeString.Equals("instant", StringComparison.InvariantCultureIgnoreCase))
                        item.EffectCastingTime = 0;
                    else
                        item.EffectCastingTime = float.Parse(castingTimeString);
                }

                // set item.EffectMinimumLevel
                if (lineChunks[currentIndex].Equals("level", StringComparison.InvariantCultureIgnoreCase) && lineChunks.Length > (currentIndex + 1))
                {
                    currentIndex++;
                    item.EffectMinimumLevel = int.Parse(lineChunks[currentIndex]);
                }

                currentIndex++;
            }
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


