using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaUpdateDto
    {
        [Required]
        public  int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public double Fee { get; set; }

        [Required]
        public int Occupants { get; set; }

        [Required]
        public int SquareMeters { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Amenity { get; set; }
    }
}
