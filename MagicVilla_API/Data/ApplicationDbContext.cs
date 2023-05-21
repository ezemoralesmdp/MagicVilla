using MagicVilla_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserApplication>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserApplication> UsersApplication { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Villa Real",
                    Details = "Villa details...",
                    ImageUrl = "",
                    Occupants = 5,
                    SquareMeters = 50,
                    Fee = 200,
                    Amenity = "",
                    DateInsert = DateTime.Now,
                    DateUpdate = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Premium Vista a la Piscina",
                    Details = "Villa details...",
                    ImageUrl = "",
                    Occupants = 4,
                    SquareMeters = 40,
                    Fee = 150,
                    Amenity = "",
                    DateInsert = DateTime.Now,
                    DateUpdate = DateTime.Now
                }
            );
        }
    }
}
