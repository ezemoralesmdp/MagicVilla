using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AddVillaNumberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VillaNumbers",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateInsert = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaNumbers", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_VillaNumbers_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 8, 9, 36, 48, 87, DateTimeKind.Local).AddTicks(3643), new DateTime(2023, 5, 8, 9, 36, 48, 87, DateTimeKind.Local).AddTicks(3652) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 8, 9, 36, 48, 87, DateTimeKind.Local).AddTicks(3656), new DateTime(2023, 5, 8, 9, 36, 48, 87, DateTimeKind.Local).AddTicks(3657) });

            migrationBuilder.CreateIndex(
                name: "IX_VillaNumbers_VillaId",
                table: "VillaNumbers",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaNumbers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5081), new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5106) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5111), new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5112) });
        }
    }
}
