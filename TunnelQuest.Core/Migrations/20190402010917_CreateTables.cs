using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TunnelQuest.Core.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alias",
                columns: table => new
                {
                    alias_text = table.Column<string>(nullable: false),
                    item_name = table.Column<string>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alias", x => x.alias_text);
                });

            migrationBuilder.CreateTable(
                name: "auth_token_status",
                columns: table => new
                {
                    auth_token_status_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_token_status", x => x.auth_token_status_code);
                });

            migrationBuilder.CreateTable(
                name: "chat_line_token_type",
                columns: table => new
                {
                    token_type_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_line_token_type", x => x.token_type_code);
                });

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    class_code = table.Column<string>(nullable: false),
                    class_name = table.Column<string>(nullable: false)
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
                    race_name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_race", x => x.race_code);
                });

            migrationBuilder.CreateTable(
                name: "server",
                columns: table => new
                {
                    server_code = table.Column<string>(nullable: false),
                    server_name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server", x => x.server_code);
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
                name: "spell",
                columns: table => new
                {
                    spell_name = table.Column<string>(nullable: false),
                    icon_file_name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spell", x => x.spell_name);
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
                name: "auth_token",
                columns: table => new
                {
                    auth_token_id = table.Column<short>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: false),
                    value = table.Column<string>(nullable: false),
                    auth_token_status_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_token", x => x.auth_token_id);
                    table.ForeignKey(
                        name: "FK_auth_token_auth_token_status_auth_token_status_code",
                        column: x => x.auth_token_status_code,
                        principalTable: "auth_token_status",
                        principalColumn: "auth_token_status_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "filter_item",
                columns: table => new
                {
                    filter_item_id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    server_code = table.Column<string>(nullable: true),
                    item_name = table.Column<string>(nullable: false),
                    alias_text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filter_item", x => x.filter_item_id);
                    table.ForeignKey(
                        name: "FK_filter_item_server_server_code",
                        column: x => x.server_code,
                        principalTable: "server",
                        principalColumn: "server_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "price_history",
                columns: table => new
                {
                    price_history_id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    server_code = table.Column<string>(nullable: false),
                    item_name = table.Column<string>(nullable: true),
                    one_month_median = table.Column<int>(nullable: true),
                    three_month_median = table.Column<int>(nullable: true),
                    six_month_median = table.Column<int>(nullable: true),
                    twelve_month_median = table.Column<int>(nullable: true),
                    lifetime_median = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_history", x => x.price_history_id);
                    table.ForeignKey(
                        name: "FK_price_history_server_server_code",
                        column: x => x.server_code,
                        principalTable: "server",
                        principalColumn: "server_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "spell_effect_detail",
                columns: table => new
                {
                    spell_effect_detail_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    spell_name = table.Column<string>(nullable: false),
                    text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spell_effect_detail", x => x.spell_effect_detail_id);
                    table.ForeignKey(
                        name: "FK_spell_effect_detail_spell_spell_name",
                        column: x => x.spell_name,
                        principalTable: "spell",
                        principalColumn: "spell_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "spell_requirement",
                columns: table => new
                {
                    spell_name = table.Column<string>(nullable: false),
                    class_code = table.Column<string>(nullable: false),
                    required_level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spell_requirement", x => new { x.spell_name, x.class_code });
                    table.ForeignKey(
                        name: "FK_spell_requirement_class_class_code",
                        column: x => x.class_code,
                        principalTable: "class",
                        principalColumn: "class_code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_spell_requirement_spell_spell_name",
                        column: x => x.spell_name,
                        principalTable: "spell",
                        principalColumn: "spell_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "spell_source",
                columns: table => new
                {
                    spell_source_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    spell_name = table.Column<string>(nullable: false),
                    text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spell_source", x => x.spell_source_id);
                    table.ForeignKey(
                        name: "FK_spell_source_spell_spell_name",
                        column: x => x.spell_name,
                        principalTable: "spell",
                        principalColumn: "spell_name",
                        onDelete: ReferentialAction.Cascade);
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
                    is_no_trade = table.Column<bool>(nullable: false),
                    is_temporary = table.Column<bool>(nullable: false),
                    is_quest_item = table.Column<bool>(nullable: false),
                    is_artifact = table.Column<bool>(nullable: false),
                    required_level = table.Column<int>(nullable: true),
                    weight = table.Column<float>(nullable: false),
                    size_code = table.Column<string>(nullable: true),
                    strength = table.Column<int>(nullable: true),
                    stamina = table.Column<int>(nullable: true),
                    agility = table.Column<int>(nullable: true),
                    dexterity = table.Column<int>(nullable: true),
                    wisdom = table.Column<int>(nullable: true),
                    intelligence = table.Column<int>(nullable: true),
                    charisma = table.Column<int>(nullable: true),
                    hit_points = table.Column<int>(nullable: true),
                    mana = table.Column<int>(nullable: true),
                    armor_class = table.Column<int>(nullable: true),
                    magic_resist = table.Column<int>(nullable: true),
                    poison_resist = table.Column<int>(nullable: true),
                    disease_resist = table.Column<int>(nullable: true),
                    fire_resist = table.Column<int>(nullable: true),
                    cold_resist = table.Column<int>(nullable: true),
                    haste = table.Column<float>(nullable: true),
                    singing_modifier = table.Column<int>(nullable: true),
                    percussion_modifier = table.Column<int>(nullable: true),
                    stringed_modifier = table.Column<int>(nullable: true),
                    brass_modifier = table.Column<int>(nullable: true),
                    wind_modifier = table.Column<int>(nullable: true),
                    effect_spell_name = table.Column<string>(nullable: true),
                    effect_type_code = table.Column<string>(nullable: true),
                    effect_minimum_level = table.Column<int>(nullable: true),
                    effect_casting_time = table.Column<float>(nullable: true),
                    weapon_skill_code = table.Column<string>(nullable: true),
                    attack_damage = table.Column<int>(nullable: true),
                    attack_delay = table.Column<int>(nullable: true),
                    range = table.Column<int>(nullable: true),
                    capacity = table.Column<int>(nullable: true),
                    capacity_size_code = table.Column<string>(nullable: true),
                    weight_reduction = table.Column<float>(nullable: true),
                    is_expendable = table.Column<bool>(nullable: true),
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
                        name: "FK_item_spell_effect_spell_name",
                        column: x => x.effect_spell_name,
                        principalTable: "spell",
                        principalColumn: "spell_name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_effect_type_effect_type_code",
                        column: x => x.effect_type_code,
                        principalTable: "effect_type",
                        principalColumn: "effect_type_code",
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
                name: "chat_line",
                columns: table => new
                {
                    chat_line_id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    auth_token_id = table.Column<short>(nullable: false),
                    server_code = table.Column<string>(nullable: false),
                    player_name = table.Column<string>(nullable: false),
                    text = table.Column<string>(nullable: false),
                    sent_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_line", x => x.chat_line_id);
                    table.ForeignKey(
                        name: "FK_chat_line_auth_token_auth_token_id",
                        column: x => x.auth_token_id,
                        principalTable: "auth_token",
                        principalColumn: "auth_token_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chat_line_server_server_code",
                        column: x => x.server_code,
                        principalTable: "server",
                        principalColumn: "server_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_class",
                columns: table => new
                {
                    item_name = table.Column<string>(nullable: false),
                    class_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_class", x => new { x.item_name, x.class_code });
                    table.ForeignKey(
                        name: "FK_item_class_class_class_code",
                        column: x => x.class_code,
                        principalTable: "class",
                        principalColumn: "class_code",
                        onDelete: ReferentialAction.Cascade);
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
                    item_name = table.Column<string>(nullable: false),
                    deity_name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_deity", x => new { x.item_name, x.deity_name });
                    table.ForeignKey(
                        name: "FK_item_deity_deity_deity_name",
                        column: x => x.deity_name,
                        principalTable: "deity",
                        principalColumn: "deity_name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_deity_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_info_line",
                columns: table => new
                {
                    item_info_line_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_name = table.Column<string>(nullable: false),
                    text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_info_line", x => x.item_info_line_id);
                    table.ForeignKey(
                        name: "FK_item_info_line_item_item_name",
                        column: x => x.item_name,
                        principalTable: "item",
                        principalColumn: "item_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_race",
                columns: table => new
                {
                    item_name = table.Column<string>(nullable: false),
                    race_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_race", x => new { x.item_name, x.race_code });
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_slot",
                columns: table => new
                {
                    item_name = table.Column<string>(nullable: false),
                    slot_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_slot", x => new { x.item_name, x.slot_code });
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auction",
                columns: table => new
                {
                    auction_id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    server_code = table.Column<string>(nullable: false),
                    player_name = table.Column<string>(nullable: false),
                    previous_auction_id = table.Column<long>(nullable: true),
                    most_recent_chat_line_id = table.Column<long>(nullable: false),
                    item_name = table.Column<string>(nullable: false),
                    is_known_item = table.Column<bool>(nullable: false),
                    is_buying = table.Column<bool>(nullable: false),
                    price = table.Column<int>(nullable: true),
                    is_or_best_offer = table.Column<bool>(nullable: false),
                    is_accepting_trades = table.Column<bool>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction", x => x.auction_id);
                    table.ForeignKey(
                        name: "FK_auction_chat_line_most_recent_chat_line_id",
                        column: x => x.most_recent_chat_line_id,
                        principalTable: "chat_line",
                        principalColumn: "chat_line_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_auction_auction_previous_auction_id",
                        column: x => x.previous_auction_id,
                        principalTable: "auction",
                        principalColumn: "auction_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_auction_server_server_code",
                        column: x => x.server_code,
                        principalTable: "server",
                        principalColumn: "server_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_line_token",
                columns: table => new
                {
                    chat_line_token_id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    chat_line_id = table.Column<long>(nullable: false),
                    token_type_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_line_token", x => x.chat_line_token_id);
                    table.ForeignKey(
                        name: "FK_chat_line_token_chat_line_chat_line_id",
                        column: x => x.chat_line_id,
                        principalTable: "chat_line",
                        principalColumn: "chat_line_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chat_line_token_chat_line_token_type_token_type_code",
                        column: x => x.token_type_code,
                        principalTable: "chat_line_token_type",
                        principalColumn: "token_type_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_line_token_property",
                columns: table => new
                {
                    chat_line_token_id = table.Column<long>(nullable: false),
                    property = table.Column<string>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_line_token_property", x => new { x.chat_line_token_id, x.property });
                    table.ForeignKey(
                        name: "FK_chat_line_token_property_chat_line_token_chat_line_token_id",
                        column: x => x.chat_line_token_id,
                        principalTable: "chat_line_token",
                        principalColumn: "chat_line_token_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alias_item_name",
                table: "alias",
                column: "item_name");

            migrationBuilder.CreateIndex(
                name: "IX_auction_most_recent_chat_line_id",
                table: "auction",
                column: "most_recent_chat_line_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_previous_auction_id",
                table: "auction",
                column: "previous_auction_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_server_code_updated_at",
                table: "auction",
                columns: new[] { "server_code", "updated_at" });

            migrationBuilder.CreateIndex(
                name: "IX_auction_server_code_item_name_player_name_updated_at",
                table: "auction",
                columns: new[] { "server_code", "item_name", "player_name", "updated_at" });

            migrationBuilder.CreateIndex(
                name: "IX_auth_token_auth_token_status_code",
                table: "auth_token",
                column: "auth_token_status_code");

            migrationBuilder.CreateIndex(
                name: "IX_chat_line_auth_token_id",
                table: "chat_line",
                column: "auth_token_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_line_server_code_chat_line_id",
                table: "chat_line",
                columns: new[] { "server_code", "chat_line_id" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_line_token_chat_line_id",
                table: "chat_line_token",
                column: "chat_line_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_line_token_token_type_code",
                table: "chat_line_token",
                column: "token_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_chat_line_token_property_chat_line_token_id",
                table: "chat_line_token_property",
                column: "chat_line_token_id");

            migrationBuilder.CreateIndex(
                name: "IX_filter_item_server_code_item_name",
                table: "filter_item",
                columns: new[] { "server_code", "item_name" });

            migrationBuilder.CreateIndex(
                name: "IX_item_capacity_size_code",
                table: "item",
                column: "capacity_size_code");

            migrationBuilder.CreateIndex(
                name: "IX_item_effect_spell_name",
                table: "item",
                column: "effect_spell_name");

            migrationBuilder.CreateIndex(
                name: "IX_item_effect_type_code",
                table: "item",
                column: "effect_type_code");

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
                name: "IX_item_info_line_item_name",
                table: "item_info_line",
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
                name: "IX_price_history_server_code_item_name",
                table: "price_history",
                columns: new[] { "server_code", "item_name" });

            migrationBuilder.CreateIndex(
                name: "IX_spell_effect_detail_spell_name",
                table: "spell_effect_detail",
                column: "spell_name");

            migrationBuilder.CreateIndex(
                name: "IX_spell_requirement_class_code",
                table: "spell_requirement",
                column: "class_code");

            migrationBuilder.CreateIndex(
                name: "IX_spell_requirement_spell_name",
                table: "spell_requirement",
                column: "spell_name");

            migrationBuilder.CreateIndex(
                name: "IX_spell_source_spell_name",
                table: "spell_source",
                column: "spell_name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alias");

            migrationBuilder.DropTable(
                name: "auction");

            migrationBuilder.DropTable(
                name: "chat_line_token_property");

            migrationBuilder.DropTable(
                name: "filter_item");

            migrationBuilder.DropTable(
                name: "item_class");

            migrationBuilder.DropTable(
                name: "item_deity");

            migrationBuilder.DropTable(
                name: "item_info_line");

            migrationBuilder.DropTable(
                name: "item_race");

            migrationBuilder.DropTable(
                name: "item_slot");

            migrationBuilder.DropTable(
                name: "price_history");

            migrationBuilder.DropTable(
                name: "spell_effect_detail");

            migrationBuilder.DropTable(
                name: "spell_requirement");

            migrationBuilder.DropTable(
                name: "spell_source");

            migrationBuilder.DropTable(
                name: "chat_line_token");

            migrationBuilder.DropTable(
                name: "deity");

            migrationBuilder.DropTable(
                name: "race");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "slot");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "chat_line");

            migrationBuilder.DropTable(
                name: "chat_line_token_type");

            migrationBuilder.DropTable(
                name: "size");

            migrationBuilder.DropTable(
                name: "spell");

            migrationBuilder.DropTable(
                name: "effect_type");

            migrationBuilder.DropTable(
                name: "weapon_skill");

            migrationBuilder.DropTable(
                name: "auth_token");

            migrationBuilder.DropTable(
                name: "server");

            migrationBuilder.DropTable(
                name: "auth_token_status");
        }
    }
}
