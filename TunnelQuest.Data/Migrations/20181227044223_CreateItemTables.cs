using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TunnelQuest.Data.Migrations
{
    public partial class CreateItemTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    class_code = table.Column<string>(nullable: false),
                    class_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class", x => x.class_code);
                });

            migrationBuilder.CreateTable(
                name: "deity",
                columns: table => new
                {
                    deity_name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deity", x => x.deity_name);
                });

            migrationBuilder.CreateTable(
                name: "effect",
                columns: table => new
                {
                    effect_name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_effect", x => x.effect_name);
                });

            migrationBuilder.CreateTable(
                name: "effect_type",
                columns: table => new
                {
                    effect_type_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_effect_type", x => x.effect_type_code);
                });

            migrationBuilder.CreateTable(
                name: "race",
                columns: table => new
                {
                    race_code = table.Column<string>(nullable: false),
                    race_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_race", x => x.race_code);
                });

            migrationBuilder.CreateTable(
                name: "size",
                columns: table => new
                {
                    size_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_size", x => x.size_code);
                });

            migrationBuilder.CreateTable(
                name: "slot",
                columns: table => new
                {
                    slot_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_slot", x => x.slot_code);
                });

            migrationBuilder.CreateTable(
                name: "stat",
                columns: table => new
                {
                    stat_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stat", x => x.stat_code);
                });

            migrationBuilder.CreateTable(
                name: "weapon_skill",
                columns: table => new
                {
                    weapon_skill_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapon_skill", x => x.weapon_skill_code);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    item_name = table.Column<string>(nullable: false),
                    icon_file_name = table.Column<string>(nullable: true),
                    is_magic = table.Column<bool>(nullable: false),
                    is_lore = table.Column<bool>(nullable: false),
                    is_no_drop = table.Column<bool>(nullable: false),
                    is_temporary = table.Column<bool>(nullable: false),
                    is_quest_item = table.Column<bool>(nullable: false),
                    weight = table.Column<float>(nullable: false),
                    size_code = table.Column<string>(nullable: true),
                    weapon_skill_code = table.Column<string>(nullable: true),
                    attack_damage = table.Column<int>(nullable: true),
                    attack_delay = table.Column<int>(nullable: true),
                    capacity = table.Column<int>(nullable: true),
                    capacity_size_code = table.Column<string>(nullable: true),
                    weight_reduction = table.Column<float>(nullable: true),
                    is_expendable = table.Column<bool>(nullable: false),
                    max_charges = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item", x => x.item_name);
                    table.ForeignKey(
                        name: "FK_item_size_capacity_size_code",
                        column: x => x.capacity_size_code,
                        principalTable: "size",
                        principalColumn: "size_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_size_size_code",
                        column: x => x.size_code,
                        principalTable: "size",
                        principalColumn: "size_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_weapon_skill_weapon_skill_code",
                        column: x => x.weapon_skill_code,
                        principalTable: "weapon_skill",
                        principalColumn: "weapon_skill_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_class",
                columns: table => new
                {
                    item_class_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_name = table.Column<string>(nullable: true),
                    class_code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_class", x => x.item_class_id);
                    table.ForeignKey(
                        name: "FK_item_class_class_class_code",
                        column: x => x.class_code,
                        principalTable: "class",
                        principalColumn: "class_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_class_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_deity",
                columns: table => new
                {
                    item_deity_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_name = table.Column<string>(nullable: true),
                    deity_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_deity", x => x.item_deity_id);
                    table.ForeignKey(
                        name: "FK_item_deity_deity_deity_name",
                        column: x => x.deity_name,
                        principalTable: "deity",
                        principalColumn: "deity_name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_deity_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_effect",
                columns: table => new
                {
                    item_effect_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_name = table.Column<string>(nullable: true),
                    effect_name = table.Column<string>(nullable: true),
                    effect_type_code = table.Column<string>(nullable: true),
                    minimum_level = table.Column<int>(nullable: true),
                    casting_time = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_effect", x => x.item_effect_id);
                    table.ForeignKey(
                        name: "FK_item_effect_effect_effect_name",
                        column: x => x.effect_name,
                        principalTable: "effect",
                        principalColumn: "effect_name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_effect_effect_type_effect_type_code",
                        column: x => x.effect_type_code,
                        principalTable: "effect_type",
                        principalColumn: "effect_type_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_effect_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_race",
                columns: table => new
                {
                    item_race_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_name = table.Column<string>(nullable: true),
                    race_code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_race", x => x.item_race_id);
                    table.ForeignKey(
                        name: "FK_item_race_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_race_race_race_code",
                        column: x => x.race_code,
                        principalTable: "race",
                        principalColumn: "race_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_slot",
                columns: table => new
                {
                    item_slot_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_name = table.Column<string>(nullable: true),
                    slot_code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_slot", x => x.item_slot_id);
                    table.ForeignKey(
                        name: "FK_item_slot_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_slot_slot_slot_code",
                        column: x => x.slot_code,
                        principalTable: "slot",
                        principalColumn: "slot_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_stat",
                columns: table => new
                {
                    item_stat_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_name = table.Column<string>(nullable: true),
                    stat_code = table.Column<string>(nullable: true),
                    adjustment = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_stat", x => x.item_stat_id);
                    table.ForeignKey(
                        name: "FK_item_stat_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_stat_stat_stat_code",
                        column: x => x.stat_code,
                        principalTable: "stat",
                        principalColumn: "stat_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_capacity_size_code",
                table: "item",
                column: "capacity_size_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_size_code",
                table: "item",
                column: "size_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_weapon_skill_code",
                table: "item",
                column: "weapon_skill_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_class_class_code",
                table: "item_class",
                column: "class_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_class_item_name",
                table: "item_class",
                column: "item_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_deity_deity_name",
                table: "item_deity",
                column: "deity_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_deity_item_name",
                table: "item_deity",
                column: "item_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_effect_effect_name",
                table: "item_effect",
                column: "effect_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_effect_effect_type_code",
                table: "item_effect",
                column: "effect_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_effect_item_name",
                table: "item_effect",
                column: "item_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_race_item_name",
                table: "item_race",
                column: "item_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_race_race_code",
                table: "item_race",
                column: "race_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_slot_item_name",
                table: "item_slot",
                column: "item_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_slot_slot_code",
                table: "item_slot",
                column: "slot_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_stat_item_name",
                table: "item_stat",
                column: "item_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_stat_stat_code",
                table: "item_stat",
                column: "stat_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_class");

            migrationBuilder.DropTable(
                name: "item_deity");

            migrationBuilder.DropTable(
                name: "item_effect");

            migrationBuilder.DropTable(
                name: "item_race");

            migrationBuilder.DropTable(
                name: "item_slot");

            migrationBuilder.DropTable(
                name: "item_stat");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "deity");

            migrationBuilder.DropTable(
                name: "effect");

            migrationBuilder.DropTable(
                name: "effect_type");

            migrationBuilder.DropTable(
                name: "race");

            migrationBuilder.DropTable(
                name: "slot");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "stat");

            migrationBuilder.DropTable(
                name: "size");

            migrationBuilder.DropTable(
                name: "weapon_skill");
        }
    }
}
