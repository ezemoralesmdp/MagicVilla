using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel
{
    public class VillaNumberUpdateViewModel
    {
        public VillaNumberUpdateViewModel()
        {
            VillaNumber = new VillaNumberUpdateDto();
        }

        public VillaNumberUpdateDto VillaNumber { get; set; }

        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
