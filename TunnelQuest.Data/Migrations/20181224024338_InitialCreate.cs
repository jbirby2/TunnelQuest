using Microsoft.EntityFrameworkCore.Migrations;

namespace TunnelQuest.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Effect",
                columns: table => new
                {
                    EffectName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effect", x => x.EffectName);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    SizeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.SizeName);
                });

            migrationBuilder.CreateTable(
                name: "Stat",
                columns: table => new
                {
                    StatCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stat", x => x.StatCode);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemName = table.Column<string>(nullable: false),
                    IsMagic = table.Column<short>(nullable: false),
                    IsLore = table.Column<short>(nullable: false),
                    IsTemporary = table.Column<short>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    SizeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemName);
                    table.ForeignKey(
                        name: "FK_Items_Size_SizeName",
                        column: x => x.SizeName,
                        principalTable: "Size",
                        principalColumn: "SizeName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    ClassCode = table.Column<string>(nullable: false),
                    ItemName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.ClassCode);
                    table.ForeignKey(
                        name: "FK_Class_Items_ItemName",
                        column: x => x.ItemName,
                        principalTable: "Items",
                        principalColumn: "ItemName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemEffect",
                columns: table => new
                {
                    ItemEffectId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ItemName = table.Column<string>(nullable: true),
                    EffectName = table.Column<string>(nullable: true),
                    EffectType = table.Column<int>(nullable: false),
                    RequiredLevel = table.Column<int>(nullable: true),
                    CastingTime = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemEffect", x => x.ItemEffectId);
                    table.ForeignKey(
                        name: "FK_ItemEffect_Effect_EffectName",
                        column: x => x.EffectName,
                        principalTable: "Effect",
                        principalColumn: "EffectName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemEffect_Items_ItemName",
                        column: x => x.ItemName,
                        principalTable: "Items",
                        principalColumn: "ItemName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemStat",
                columns: table => new
                {
                    ItemStatId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ItemName = table.Column<string>(nullable: true),
                    StatCode = table.Column<string>(nullable: true),
                    Adjustment = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStat", x => x.ItemStatId);
                    table.ForeignKey(
                        name: "FK_ItemStat_Items_ItemName",
                        column: x => x.ItemName,
                        principalTable: "Items",
                        principalColumn: "ItemName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemStat_Stat_StatCode",
                        column: x => x.StatCode,
                        principalTable: "Stat",
                        principalColumn: "StatCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Race",
                columns: table => new
                {
                    RaceCode = table.Column<string>(nullable: false),
                    ItemName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Race", x => x.RaceCode);
                    table.ForeignKey(
                        name: "FK_Race_Items_ItemName",
                        column: x => x.ItemName,
                        principalTable: "Items",
                        principalColumn: "ItemName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Class_ItemName",
                table: "Class",
                column: "ItemName");

            migrationBuilder.CreateIndex(
                name: "IX_ItemEffect_EffectName",
                table: "ItemEffect",
                column: "EffectName");

            migrationBuilder.CreateIndex(
                name: "IX_ItemEffect_ItemName",
                table: "ItemEffect",
                column: "ItemName");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SizeName",
                table: "Items",
                column: "SizeName");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStat_ItemName",
                table: "ItemStat",
                column: "ItemName");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStat_StatCode",
                table: "ItemStat",
                column: "StatCode");

            migrationBuilder.CreateIndex(
                name: "IX_Race_ItemName",
                table: "Race",
                column: "ItemName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "ItemEffect");

            migrationBuilder.DropTable(
                name: "ItemStat");

            migrationBuilder.DropTable(
                name: "Race");

            migrationBuilder.DropTable(
                name: "Effect");

            migrationBuilder.DropTable(
                name: "Stat");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Size");
        }
    }
}
