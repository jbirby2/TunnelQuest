﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Core.Migrations
{
    [DbContext(typeof(TunnelQuestContext))]
    partial class TunnelQuestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TunnelQuest.Core.Models.Auction", b =>
                {
                    b.Property<long>("AuctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("auction_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsAcceptingTrades")
                        .HasColumnName("is_accepting_trades");

                    b.Property<bool>("IsBuying")
                        .HasColumnName("is_buying");

                    b.Property<bool>("IsKnownItem")
                        .HasColumnName("is_known_item");

                    b.Property<bool>("IsOrBestOffer")
                        .HasColumnName("is_or_best_offer");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnName("item_name");

                    b.Property<long>("MostRecentChatLineId")
                        .HasColumnName("most_recent_chat_line_id");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnName("player_name");

                    b.Property<long?>("PreviousAuctionId")
                        .HasColumnName("previous_auction_id");

                    b.Property<int?>("Price")
                        .HasColumnName("price");

                    b.Property<string>("ServerCode")
                        .IsRequired()
                        .HasColumnName("server_code");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at");

                    b.HasKey("AuctionId");

                    b.HasIndex("MostRecentChatLineId");

                    b.HasIndex("PreviousAuctionId");

                    b.HasIndex("ServerCode", "UpdatedAt");

                    b.HasIndex("ServerCode", "ItemName", "PlayerName", "UpdatedAt");

                    b.ToTable("auction");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.AuthToken", b =>
                {
                    b.Property<short>("AuthTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("auth_token_id");

                    b.Property<string>("AuthTokenStatusCode")
                        .IsRequired()
                        .HasColumnName("auth_token_status_code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("value");

                    b.HasKey("AuthTokenId");

                    b.HasIndex("AuthTokenStatusCode");

                    b.ToTable("auth_token");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.AuthTokenStatus", b =>
                {
                    b.Property<string>("AuthTokenStatusCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("auth_token_status_code");

                    b.HasKey("AuthTokenStatusCode");

                    b.ToTable("auth_token_status");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ChatLine", b =>
                {
                    b.Property<long>("ChatLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("chat_line_id");

                    b.Property<short>("AuthTokenId")
                        .HasColumnName("auth_token_id");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnName("player_name");

                    b.Property<DateTime>("SentAt")
                        .HasColumnName("sent_at");

                    b.Property<string>("ServerCode")
                        .IsRequired()
                        .HasColumnName("server_code");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text");

                    b.HasKey("ChatLineId");

                    b.HasIndex("AuthTokenId");

                    b.HasIndex("ServerCode", "ChatLineId");

                    b.ToTable("chat_line");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ChatLineToken", b =>
                {
                    b.Property<long>("ChatLineTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("chat_line_token_id");

                    b.Property<long>("ChatLineId")
                        .HasColumnName("chat_line_id");

                    b.Property<string>("TokenTypeCode")
                        .IsRequired()
                        .HasColumnName("token_type_code");

                    b.HasKey("ChatLineTokenId");

                    b.HasIndex("ChatLineId");

                    b.HasIndex("TokenTypeCode");

                    b.ToTable("chat_line_token");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ChatLineTokenProperty", b =>
                {
                    b.Property<long>("ChatLineTokenId")
                        .HasColumnName("chat_line_token_id");

                    b.Property<string>("Property")
                        .HasColumnName("property");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("value");

                    b.HasKey("ChatLineTokenId", "Property");

                    b.HasIndex("ChatLineTokenId");

                    b.ToTable("chat_line_token_property");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ChatLineTokenType", b =>
                {
                    b.Property<string>("TokenTypeCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("token_type_code");

                    b.HasKey("TokenTypeCode");

                    b.ToTable("chat_line_token_type");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Class", b =>
                {
                    b.Property<string>("ClassCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("class_code");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnName("class_name");

                    b.HasKey("ClassCode");

                    b.ToTable("class");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Deity", b =>
                {
                    b.Property<string>("DeityName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("deity_name");

                    b.HasKey("DeityName");

                    b.ToTable("deity");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.EffectType", b =>
                {
                    b.Property<string>("EffectTypeCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("effect_type_code");

                    b.HasKey("EffectTypeCode");

                    b.ToTable("effect_type");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Item", b =>
                {
                    b.Property<string>("ItemName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("item_name");

                    b.Property<int?>("Agility")
                        .HasColumnName("agility");

                    b.Property<int?>("ArmorClass")
                        .HasColumnName("armor_class");

                    b.Property<int?>("AttackDamage")
                        .HasColumnName("attack_damage");

                    b.Property<int?>("AttackDelay")
                        .HasColumnName("attack_delay");

                    b.Property<int?>("BrassModifier")
                        .HasColumnName("brass_modifier");

                    b.Property<int?>("Capacity")
                        .HasColumnName("capacity");

                    b.Property<string>("CapacitySizeCode")
                        .HasColumnName("capacity_size_code");

                    b.Property<int?>("Charisma")
                        .HasColumnName("charisma");

                    b.Property<int?>("ColdResist")
                        .HasColumnName("cold_resist");

                    b.Property<int?>("Dexterity")
                        .HasColumnName("dexterity");

                    b.Property<int?>("DiseaseResist")
                        .HasColumnName("disease_resist");

                    b.Property<float?>("EffectCastingTime")
                        .HasColumnName("effect_casting_time");

                    b.Property<int?>("EffectMinimumLevel")
                        .HasColumnName("effect_minimum_level");

                    b.Property<string>("EffectSpellName")
                        .HasColumnName("effect_spell_name");

                    b.Property<string>("EffectTypeCode")
                        .HasColumnName("effect_type_code");

                    b.Property<int?>("FireResist")
                        .HasColumnName("fire_resist");

                    b.Property<float?>("Haste")
                        .HasColumnName("haste");

                    b.Property<int?>("HitPoints")
                        .HasColumnName("hit_points");

                    b.Property<string>("IconFileName")
                        .HasColumnName("icon_file_name");

                    b.Property<int?>("Intelligence")
                        .HasColumnName("intelligence");

                    b.Property<bool>("IsArtifact")
                        .HasColumnName("is_artifact");

                    b.Property<bool?>("IsExpendable")
                        .HasColumnName("is_expendable");

                    b.Property<bool>("IsLore")
                        .HasColumnName("is_lore");

                    b.Property<bool>("IsMagic")
                        .HasColumnName("is_magic");

                    b.Property<bool>("IsNoDrop")
                        .HasColumnName("is_no_drop");

                    b.Property<bool>("IsNoTrade")
                        .HasColumnName("is_no_trade");

                    b.Property<bool>("IsQuestItem")
                        .HasColumnName("is_quest_item");

                    b.Property<bool>("IsTemporary")
                        .HasColumnName("is_temporary");

                    b.Property<int?>("MagicResist")
                        .HasColumnName("magic_resist");

                    b.Property<int?>("Mana")
                        .HasColumnName("mana");

                    b.Property<int?>("MaxCharges")
                        .HasColumnName("max_charges");

                    b.Property<int?>("PercussionModifier")
                        .HasColumnName("percussion_modifier");

                    b.Property<int?>("PoisonResist")
                        .HasColumnName("poison_resist");

                    b.Property<int?>("Range")
                        .HasColumnName("range");

                    b.Property<int?>("RequiredLevel")
                        .HasColumnName("required_level");

                    b.Property<int?>("SingingModifier")
                        .HasColumnName("singing_modifier");

                    b.Property<string>("SizeCode")
                        .HasColumnName("size_code");

                    b.Property<int?>("Stamina")
                        .HasColumnName("stamina");

                    b.Property<int?>("Strength")
                        .HasColumnName("strength");

                    b.Property<int?>("StringedModifier")
                        .HasColumnName("stringed_modifier");

                    b.Property<string>("WeaponSkillCode")
                        .HasColumnName("weapon_skill_code");

                    b.Property<float>("Weight")
                        .HasColumnName("weight");

                    b.Property<float?>("WeightReduction")
                        .HasColumnName("weight_reduction");

                    b.Property<int?>("WindModifier")
                        .HasColumnName("wind_modifier");

                    b.Property<int?>("Wisdom")
                        .HasColumnName("wisdom");

                    b.HasKey("ItemName");

                    b.HasIndex("CapacitySizeCode");

                    b.HasIndex("EffectSpellName");

                    b.HasIndex("EffectTypeCode");

                    b.HasIndex("SizeCode");

                    b.HasIndex("WeaponSkillCode");

                    b.ToTable("item");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemClass", b =>
                {
                    b.Property<string>("ItemName")
                        .HasColumnName("item_name");

                    b.Property<string>("ClassCode")
                        .HasColumnName("class_code");

                    b.HasKey("ItemName", "ClassCode");

                    b.HasIndex("ClassCode");

                    b.HasIndex("ItemName");

                    b.ToTable("item_class");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemDeity", b =>
                {
                    b.Property<string>("ItemName")
                        .HasColumnName("item_name");

                    b.Property<string>("DeityName")
                        .HasColumnName("deity_name");

                    b.HasKey("ItemName", "DeityName");

                    b.HasIndex("DeityName");

                    b.HasIndex("ItemName");

                    b.ToTable("item_deity");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemInfoLine", b =>
                {
                    b.Property<int>("ItemInfoLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("item_info_line_id");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnName("item_name");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text");

                    b.HasKey("ItemInfoLineId");

                    b.HasIndex("ItemName");

                    b.ToTable("item_info_line");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemRace", b =>
                {
                    b.Property<string>("ItemName")
                        .HasColumnName("item_name");

                    b.Property<string>("RaceCode")
                        .HasColumnName("race_code");

                    b.HasKey("ItemName", "RaceCode");

                    b.HasIndex("ItemName");

                    b.HasIndex("RaceCode");

                    b.ToTable("item_race");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemSlot", b =>
                {
                    b.Property<string>("ItemName")
                        .HasColumnName("item_name");

                    b.Property<string>("SlotCode")
                        .HasColumnName("slot_code");

                    b.HasKey("ItemName", "SlotCode");

                    b.HasIndex("ItemName");

                    b.HasIndex("SlotCode");

                    b.ToTable("item_slot");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.PriceHistory", b =>
                {
                    b.Property<string>("ItemName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("item_name");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<int>("LifetimeMedian")
                        .HasColumnName("lifetime_median");

                    b.Property<int?>("OneMonthMedian")
                        .HasColumnName("one_month_median");

                    b.Property<int?>("SixMonthMedian")
                        .HasColumnName("six_month_median");

                    b.Property<int?>("ThreeMonthMedian")
                        .HasColumnName("three_month_median");

                    b.Property<int?>("TwelveMonthMedian")
                        .HasColumnName("twelve_month_median");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at");

                    b.HasKey("ItemName");

                    b.ToTable("price_history");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Race", b =>
                {
                    b.Property<string>("RaceCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("race_code");

                    b.Property<string>("RaceName")
                        .IsRequired()
                        .HasColumnName("race_name");

                    b.HasKey("RaceCode");

                    b.ToTable("race");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Server", b =>
                {
                    b.Property<string>("ServerCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("server_code");

                    b.Property<string>("ServerName")
                        .IsRequired()
                        .HasColumnName("server_name");

                    b.HasKey("ServerCode");

                    b.ToTable("server");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Size", b =>
                {
                    b.Property<string>("SizeCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("size_code");

                    b.HasKey("SizeCode");

                    b.ToTable("size");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Slot", b =>
                {
                    b.Property<string>("SlotCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("slot_code");

                    b.HasKey("SlotCode");

                    b.ToTable("slot");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Spell", b =>
                {
                    b.Property<string>("SpellName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("spell_name");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<string>("IconFileName")
                        .HasColumnName("icon_file_name");

                    b.HasKey("SpellName");

                    b.ToTable("spell");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.SpellEffectDetail", b =>
                {
                    b.Property<int>("SpellRequirementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("spell_effect_detail_id");

                    b.Property<string>("SpellName")
                        .IsRequired()
                        .HasColumnName("spell_name");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text");

                    b.HasKey("SpellRequirementId");

                    b.HasIndex("SpellName");

                    b.ToTable("spell_effect_detail");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.SpellRequirement", b =>
                {
                    b.Property<string>("SpellName")
                        .HasColumnName("spell_name");

                    b.Property<string>("ClassCode")
                        .HasColumnName("class_code");

                    b.Property<int>("RequiredLevel")
                        .HasColumnName("required_level");

                    b.HasKey("SpellName", "ClassCode");

                    b.HasIndex("ClassCode");

                    b.HasIndex("SpellName");

                    b.ToTable("spell_requirement");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.SpellSource", b =>
                {
                    b.Property<int>("SpellSourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("spell_source_id");

                    b.Property<string>("SpellName")
                        .IsRequired()
                        .HasColumnName("spell_name");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text");

                    b.HasKey("SpellSourceId");

                    b.HasIndex("SpellName");

                    b.ToTable("spell_source");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.WeaponSkill", b =>
                {
                    b.Property<string>("WeaponSkillCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("weapon_skill_code");

                    b.HasKey("WeaponSkillCode");

                    b.ToTable("weapon_skill");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Auction", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.ChatLine", "MostRecentChatLine")
                        .WithMany()
                        .HasForeignKey("MostRecentChatLineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.Auction", "PreviousAuction")
                        .WithMany()
                        .HasForeignKey("PreviousAuctionId");

                    b.HasOne("TunnelQuest.Core.Models.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.AuthToken", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.AuthTokenStatus", "AuthTokenStatus")
                        .WithMany()
                        .HasForeignKey("AuthTokenStatusCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ChatLine", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.AuthToken", "AuthToken")
                        .WithMany()
                        .HasForeignKey("AuthTokenId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ChatLineToken", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.ChatLine", "ChatLine")
                        .WithMany("Tokens")
                        .HasForeignKey("ChatLineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.ChatLineTokenType", "TokenType")
                        .WithMany()
                        .HasForeignKey("TokenTypeCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ChatLineTokenProperty", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.ChatLineToken", "ChatLineToken")
                        .WithMany("Properties")
                        .HasForeignKey("ChatLineTokenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.Item", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Size", "CapacitySize")
                        .WithMany()
                        .HasForeignKey("CapacitySizeCode");

                    b.HasOne("TunnelQuest.Core.Models.Spell", "EffectSpell")
                        .WithMany()
                        .HasForeignKey("EffectSpellName");

                    b.HasOne("TunnelQuest.Core.Models.EffectType", "EffectType")
                        .WithMany()
                        .HasForeignKey("EffectTypeCode");

                    b.HasOne("TunnelQuest.Core.Models.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeCode");

                    b.HasOne("TunnelQuest.Core.Models.WeaponSkill", "WeaponSkill")
                        .WithMany()
                        .HasForeignKey("WeaponSkillCode");
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemClass", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.Item", "Item")
                        .WithMany("Classes")
                        .HasForeignKey("ItemName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemDeity", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Deity", "Deity")
                        .WithMany()
                        .HasForeignKey("DeityName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.Item", "Item")
                        .WithMany("Deities")
                        .HasForeignKey("ItemName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemInfoLine", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Item", "Item")
                        .WithMany("Info")
                        .HasForeignKey("ItemName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemRace", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Item", "Item")
                        .WithMany("Races")
                        .HasForeignKey("ItemName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.Race", "Race")
                        .WithMany()
                        .HasForeignKey("RaceCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.ItemSlot", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Item", "Item")
                        .WithMany("Slots")
                        .HasForeignKey("ItemName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.Slot", "Slot")
                        .WithMany()
                        .HasForeignKey("SlotCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.SpellEffectDetail", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Spell", "Spell")
                        .WithMany("EffectDetails")
                        .HasForeignKey("SpellName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.SpellRequirement", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TunnelQuest.Core.Models.Spell", "Spell")
                        .WithMany("Requirements")
                        .HasForeignKey("SpellName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TunnelQuest.Core.Models.SpellSource", b =>
                {
                    b.HasOne("TunnelQuest.Core.Models.Spell", "Spell")
                        .WithMany("Sources")
                        .HasForeignKey("SpellName")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
