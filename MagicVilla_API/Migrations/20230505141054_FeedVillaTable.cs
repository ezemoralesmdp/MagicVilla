using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class FeedVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "DateInsert", "DateUpdate", "Details", "Fee", "ImageUrl", "Name", "Occupants", "SquareMeters" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5081), new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5106), "Villa details...", 200.0, "", "Villa Real", 5, 50 },
                    { 2, "", new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5111), new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5112), "Villa details...", 150.0, "", "Premium Vista a la Piscina", 4, 40 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
