using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class UserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Names = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 20, 18, 43, 28, 672, DateTimeKind.Local).AddTicks(5708), new DateTime(2023, 5, 20, 18, 43, 28, 672, DateTimeKind.Local).AddTicks(5720) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 20, 18, 43, 28, 672, DateTimeKind.Local).AddTicks(5723), new DateTime(2023, 5, 20, 18, 43, 28, 672, DateTimeKind.Local).AddTicks(5724) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 15, 23, 48, 39, 3, DateTimeKind.Local).AddTicks(1725), new DateTime(2023, 5, 15, 23, 48, 39, 3, DateTimeKind.Local).AddTicks(1734) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 15, 23, 48, 39, 3, DateTimeKind.Local).AddTicks(1737), new DateTime(2023, 5, 15, 23, 48, 39, 3, DateTimeKind.Local).AddTicks(1738) });
        }
    }
}
