using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel
{
    public class VillaNumberRemoveViewModel
    {
        public VillaNumberRemoveViewModel()
        {
            VillaNumber = new VillaNumberDto();
        }

        public VillaNumberDto VillaNumber { get; set; }

        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
