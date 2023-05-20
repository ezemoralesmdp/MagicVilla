using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel
{
    public class VillaNumberViewModel
    {
        public VillaNumberViewModel()
        {
            VillaNumber = new VillaNumberCreateDto();
        }

        public VillaNumberCreateDto VillaNumber { get; set; }

        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
