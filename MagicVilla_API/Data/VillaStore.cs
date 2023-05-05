using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{ Id = 1, Name = "Vista a la Piscina", Occupants = 3, SquareMeters = 50 },
            new VillaDto{ Id = 2, Name = "Vista la Playa", Occupants = 4, SquareMeters = 80 },
            new VillaDto{ Id = 3, Name = "Elsart GROSO", Occupants = 23, SquareMeters = 120 },
        };
    }
}
