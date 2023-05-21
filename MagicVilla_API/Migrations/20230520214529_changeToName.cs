using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class changeToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Names",
                table: "Users",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 20, 18, 45, 29, 474, DateTimeKind.Local).AddTicks(7929), new DateTime(2023, 5, 20, 18, 45, 29, 474, DateTimeKind.Local).AddTicks(7940) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateInsert", "DateUpdate" },
                values: new object[] { new DateTime(2023, 5, 20, 18, 45, 29, 474, DateTimeKind.Local).AddTicks(7943), new DateTime(2023, 5, 20, 18, 45, 29, 474, DateTimeKind.Local).AddTicks(7944) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Names");

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
    }
}
