using Microsoft.EntityFrameworkCore.Migrations;

namespace TunnelQuest.Data.Migrations
{
    public partial class InsertStaticData : Migration
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
    }

}

