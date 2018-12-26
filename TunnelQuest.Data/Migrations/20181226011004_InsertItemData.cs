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
            insertClasses(migrationBuilder);
            insertRaces(migrationBuilder);
            insertSizes(migrationBuilder);
            insertSlots(migrationBuilder);
            insertEffectTypes(migrationBuilder);
            insertWeaponSkills(migrationBuilder);
            insertStats(migrationBuilder);
            insertDeities(migrationBuilder);
            insertItemsAndEffects();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            deleteClasses(migrationBuilder);
            deleteRaces(migrationBuilder);
            deleteSizes(migrationBuilder);
            deleteSlots(migrationBuilder);
            deleteEffectTypes(migrationBuilder);
            deleteWeaponSkills(migrationBuilder);
            deleteStats(migrationBuilder);
            deleteDeities(migrationBuilder);
            deleteItemsAndEffects();
        }


        // private helpers

        #region class
        private void insertClasses(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "ENC", "Enchanter" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "MAG", "Magician" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "NEC", "Necromancer" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "WIZ", "Wizard" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "CLR", "Cleric" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "DRU", "Druid" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "SHM", "Shaman" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "BRD", "Bard" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "MNK", "Monk" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "RNG", "Ranger" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "ROG", "Rogue" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "PAL", "Paladin" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "SHD", "Shadow Knight" });

            migrationBuilder.InsertData(
                table: "class",
                columns: new string[] { "class_code", "class_name" },
                values: new object[] { "WAR", "Warrior" });
        }

        private void deleteClasses(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "ENC");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "MAG");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "NEC");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "WIZ");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "CLR");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "DRU");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "SHM");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "BRD");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "MNK");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "RNG");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "ROG");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "PAL");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "SHD");

            migrationBuilder.DeleteData(
                table: "class",
                keyColumn: "class_code",
                keyValue: "WAR");
        }
        #endregion

        #region race
        private void insertRaces(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "BAR", "Barbarian" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "DEF", "Dark Elf" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "DWF", "Dwarf" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "ERU", "Erudite" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "GNM", "Gnome" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "HEF", "Half-Elf" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "HFL", "Halfling" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "HIE", "High Elf" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "HUM", "Human" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "IKS", "Iksar" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "OGR", "Ogre" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "TRL", "Troll" });

            migrationBuilder.InsertData(
                table: "race",
                columns: new string[] { "race_code", "race_name" },
                values: new object[] { "ELF", "Wood Elf" });
        }

        private void deleteRaces(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "BAR");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "DEF");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "DWF");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "ERU");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "GNM");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "HEF");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "HFL");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "HIE");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "HUM");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "IKS");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "OGR");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "TRL");

            migrationBuilder.DeleteData(
                table: "race",
                keyColumn: "race_code",
                keyValue: "ELF");
        }

        #endregion

        #region size
        private void insertSizes(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "size",
                columns: new string[] { "size_code" },
                values: new object[] { "TINY" });

            migrationBuilder.InsertData(
                table: "size",
                columns: new string[] { "size_code" },
                values: new object[] { "SMALL" });

            migrationBuilder.InsertData(
                table: "size",
                columns: new string[] { "size_code" },
                values: new object[] { "MEDIUM" });

            migrationBuilder.InsertData(
                table: "size",
                columns: new string[] { "size_code" },
                values: new object[] { "LARGE" });

            migrationBuilder.InsertData(
                table: "size",
                columns: new string[] { "size_code" },
                values: new object[] { "GIANT" });
        }

        private void deleteSizes(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "size",
                keyColumn: "size_code",
                keyValue: "TINY");

            migrationBuilder.DeleteData(
                table: "size",
                keyColumn: "size_code",
                keyValue: "SMALL");

            migrationBuilder.DeleteData(
                table: "size",
                keyColumn: "size_code",
                keyValue: "MEDIUM");

            migrationBuilder.DeleteData(
                table: "size",
                keyColumn: "size_code",
                keyValue: "LARGE");

            migrationBuilder.DeleteData(
                table: "size",
                keyColumn: "size_code",
                keyValue: "GIANT");
        }
        #endregion

        #region slot
        private void insertSlots(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "ARMS" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "BACK" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "CHEST" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "EAR" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "FACE" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "FEET" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "FINGER" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "HANDS" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "HEAD" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "LEGS" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "NECK" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "SHOULDERS" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "WAIST" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "WRIST" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "PRIMARY" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "SECONDARY" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "RANGE" });

            migrationBuilder.InsertData(
                table: "slot",
                columns: new string[] { "slot_code" },
                values: new object[] { "AMMO" });
        }

        private void deleteSlots(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "ARMS");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "BACK");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "CHEST");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "EAR");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "FACE");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "FEET");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "FINGER");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "HANDS");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "HEAD");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "LEGS");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "NECK");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "SHOULDERS");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "WAIST");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "WRIST");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "PRIMARY");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "SECONDARY");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "RANGE");

            migrationBuilder.DeleteData(
                table: "slot",
                keyColumn: "slot_code",
                keyValue: "AMMO");
        }
        #endregion

        #region effect_type
        private void insertEffectTypes(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "effect_type",
                columns: new string[] { "effect_type_code" },
                values: new object[] { "Combat" });

            migrationBuilder.InsertData(
                table: "effect_type",
                columns: new string[] { "effect_type_code" },
                values: new object[] { "Worn" });

            migrationBuilder.InsertData(
                table: "effect_type",
                columns: new string[] { "effect_type_code" },
                values: new object[] { "ClickAnySlot" });

            migrationBuilder.InsertData(
                table: "effect_type",
                columns: new string[] { "effect_type_code" },
                values: new object[] { "ClickEquipped" });
        }

        private void deleteEffectTypes(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "effect_type",
                keyColumn: "effect_type_code",
                keyValue: "Combat");

            migrationBuilder.DeleteData(
                table: "effect_type",
                keyColumn: "effect_type_code",
                keyValue: "Worn");

            migrationBuilder.DeleteData(
                table: "effect_type",
                keyColumn: "effect_type_code",
                keyValue: "ClickAnySlot");

            migrationBuilder.DeleteData(
                table: "effect_type",
                keyColumn: "effect_type_code",
                keyValue: "ClickEquipped");
        }
        #endregion

        #region weapon_skill
        private void insertWeaponSkills(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "1H Blunt" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "2H Blunt" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "1H Slashing" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "2H Slashing" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "Piercing" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "2H Piercing" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "Archery" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "Throwingv2" });

            migrationBuilder.InsertData(
                table: "weapon_skill",
                columns: new string[] { "weapon_skill_code" },
                values: new object[] { "Hand to Hand" });

        }

        private void deleteWeaponSkills(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "1H Blunt");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "2H Blunt");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "1H Slashing");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "2H Slashing");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "Piercing");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "2H Piercing");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "Archery");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "Throwingv2");

            migrationBuilder.DeleteData(
                table: "weapon_skill",
                keyColumn: "weapon_skill_code",
                keyValue: "Hand to Hand");
        }
        #endregion

        #region stat
        private void insertStats(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "STR" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "STA" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "AGI" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "DEX" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "WIS" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "INT" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "CHA" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "HP" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "MANA" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "AC" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "SV MAGIC" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "SV POISON" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "SV DISEASE" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "SV FIRE" });

            migrationBuilder.InsertData(
                table: "stat",
                columns: new string[] { "stat_code" },
                values: new object[] { "SV COLD" });

        }

        private void deleteStats(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "STR");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "STA");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "AGI");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "DEX");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "WIS");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "INT");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "CHA");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "HP");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "MANA");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "AC");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "SV MAGIC");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "SV POISON");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "SV DISEASE");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "SV FIRE");

            migrationBuilder.DeleteData(
                table: "stat",
                keyColumn: "stat_code",
                keyValue: "SV COLD");
        }
        #endregion

        #region deity
        private void insertDeities(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Cazic Thule" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Tunare" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Karana" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Brell Serilis" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Innoruuk" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Quellious" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Bertoxxulous" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Erollisi Marr" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Bristlebane" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Mithaniel Marr" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Prexus" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Rallos Zek" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Rodcet Nife" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "Solusek Ro" });

            migrationBuilder.InsertData(
                table: "deity",
                columns: new string[] { "deity_name" },
                values: new object[] { "The Tribunal" });
        }

        private void deleteDeities(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Cazic Thule");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Tunare");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Karana");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Brell Serilis");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Innoruuk");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Quellious");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Bertoxxulous");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Erollisi Marr");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Bristlebane");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Mithaniel Marr");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Prexus");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Rallos Zek");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Rodcet Nife");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "Solusek Ro");

            migrationBuilder.DeleteData(
                table: "deity",
                keyColumn: "deity_name",
                keyValue: "The Tribunal");
        }
        #endregion

        #region item

        private void insertItemsAndEffects()
        {
            var wikiData = WikiItemData.ReadFromEmbeddedResource();
            var items = parseWikiItems(wikiData);

            // One entry per effect name.
            // Usage: nameNormalizer[lowerCaseName] = nameToUseInDatabase
            var effectNameNormalizer = new Dictionary<string, string>();

            // build nameNormalizer

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

                if (effectNameNormalizer.ContainsKey(lowerName))
                    throw new Exception("stub exception this shouldn't happen");

                effectNameNormalizer[lowerName] = mostPopularNameCase;
            }

            // use nameNormalizer to make sure all items use the most popular spelling of their effect names
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

            using (var context = new TunnelQuestContext())
            {
                // STUB
                foreach (var item in items)
                {
                    item.ItemEffects = null;
                    context.Items.Add(item);
                    context.SaveChanges();
                }
                return;
                // end stub

                // insert all rows into effect table
                foreach (string effectName in effectNameNormalizer.Values)
                {
                    context.Effects.Add(new Effect() {
                        EffectName = effectName
                    });
                }

                // insert all item rows
                context.Items.AddRange(items);

                context.SaveChanges();
            }
        }

        private void deleteItemsAndEffects()
        {
            var wikiData = WikiItemData.ReadFromEmbeddedResource();
            var items = parseWikiItems(wikiData);

            using (var context = new TunnelQuestContext())
            {
                // stub... more item tables...
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE item_effect");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE item");
                
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE effect");
            }
        }
        #endregion

        
        public static void STUB()
        {
            new InsertItemData().insertItemsAndEffects();
        }
        
        private List<Item> parseWikiItems(IEnumerable<WikiItemData> wikiData)
        {
            var items = new List<Item>();

            foreach (WikiItemData wikiItem in wikiData)
            {
                if (wikiItem.Stats != null && !String.IsNullOrWhiteSpace(wikiItem.ItemName))
                {
                    var item = new Item();
                    item.ItemName = wikiItem.ItemName;
                    item.IconFileName = wikiItem.IconFileName;

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

            return items;
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
    }
}

