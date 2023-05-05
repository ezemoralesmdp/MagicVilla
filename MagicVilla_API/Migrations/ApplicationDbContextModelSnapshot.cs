﻿// <auto-generated />
using System;
using MagicVilla_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVilla_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_API.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateInsert")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Fee")
                        .HasColumnType("float");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Occupants")
                        .HasColumnType("int");

                    b.Property<int>("SquareMeters")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenity = "",
                            DateInsert = new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5081),
                            DateUpdate = new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5106),
                            Details = "Villa details...",
                            Fee = 200.0,
                            ImageUrl = "",
                            Name = "Villa Real",
                            Occupants = 5,
                            SquareMeters = 50
                        },
                        new
                        {
                            Id = 2,
                            Amenity = "",
                            DateInsert = new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5111),
                            DateUpdate = new DateTime(2023, 5, 5, 11, 10, 54, 364, DateTimeKind.Local).AddTicks(5112),
                            Details = "Villa details...",
                            Fee = 150.0,
                            ImageUrl = "",
                            Name = "Premium Vista a la Piscina",
                            Occupants = 4,
                            SquareMeters = 40
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
