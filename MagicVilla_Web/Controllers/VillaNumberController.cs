using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.WebSockets;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDto> villaNumberList = new();
            var response = await _villaNumberService.GetAll<APIResponse>();

            if (response != null && response.IsSuccessful)
                villaNumberList = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result));

            return View(villaNumberList);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberViewModel villaNumberVM = new();
            var response = await _villaService.GetAll<APIResponse>();

            if(response != null && response.IsSuccessful)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result))
                                            .Select(v => new SelectListItem
                                            {
                                                Text = v.Name,
                                                Value = v.Id.ToString()
                                            });
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.Create<APIResponse>(model.VillaNumber);
                if(response != null && response.IsSuccessful)
                {
                    TempData["successful"] = $"Villa number \"{model.VillaNumber.VillaNo}\" successfully created!";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    //if(response.ErrorMessages.Count > 0)
                    //{
                    //ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    //}
                    ModelState.AddModelError("ErrorMessages", response.SingleErrorMessage);
                }
            }

            var res = await _villaService.GetAll<APIResponse>();

            if (res != null && res.IsSuccessful)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(res.Result))
                                    .Select(v => new SelectListItem
                                    {
                                        Text = v.Name,
                                        Value = v.Id.ToString()
                                    });
            }

            return View(model);
        }

		public async Task<IActionResult> UpdateVillaNumber(int villaNo)
		{
			VillaNumberUpdateViewModel villaNumberVM = new();
			var response = await _villaNumberService.Get<APIResponse>(villaNo);

			if (response != null && response.IsSuccessful)
			{
                VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(model);
			}

			response = await _villaService.GetAll<APIResponse>();

			if (response != null && response.IsSuccessful)
			{
				villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result))
											.Select(v => new SelectListItem
											{
												Text = v.Name,
												Value = v.Id.ToString()
											});
			    return View(villaNumberVM);
			}

            return NotFound();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.Update<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccessful)
                {
                    TempData["successful"] = $"Villa number \"{model.VillaNumber.VillaNo}\" successfully updated!";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    //if(response.ErrorMessages.Count > 0)
                    //{
                    //ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    //}
                    ModelState.AddModelError("ErrorMessages", response.SingleErrorMessage);
                }
            }

            var res = await _villaService.GetAll<APIResponse>();

            if (res != null && res.IsSuccessful)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(res.Result))
                                    .Select(v => new SelectListItem
                                    {
                                        Text = v.Name,
                                        Value = v.Id.ToString()
                                    });
            }

            return View(model);
        }

        public async Task<IActionResult> RemoveVillaNumber(int villaNo)
        {
            VillaNumberRemoveViewModel villaNumberVM = new();
            var response = await _villaNumberService.Get<APIResponse>(villaNo);

            if (response != null && response.IsSuccessful)
            {
                VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
            }

            response = await _villaService.GetAll<APIResponse>();

            if (response != null && response.IsSuccessful)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result))
                                            .Select(v => new SelectListItem
                                            {
                                                Text = v.Name,
                                                Value = v.Id.ToString()
                                            });
                return View(villaNumberVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveVillaNumber(VillaNumberRemoveViewModel model)
        {
            var response = await _villaNumberService.Remove<APIResponse>(model.VillaNumber.VillaNo);

            if(response != null && response.IsSuccessful)
            {
                TempData["successful"] = $"Villa number successfully removed!";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            TempData["error"] = $"Villa number could not be removed!";
            return View(model);
        }
    }
}
