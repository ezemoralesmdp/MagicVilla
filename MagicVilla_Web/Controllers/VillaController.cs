using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_Utils;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            this._villaService = villaService;
            this._mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDto> villaList = new();
            var response = await _villaService.GetAll<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsSuccessful)
                villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));

            return View(villaList);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDto model)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.Create<APIResponse>(model, HttpContext.Session.GetString(DS.SessionToken));

                if(response != null && response.IsSuccessful)
                {
                    TempData["successful"] = $"Villa \"{model.Name}\" successfully created!";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.Get<APIResponse>(villaId, HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsSuccessful)
            {
                VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpdateDto>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.Update<APIResponse>(model, HttpContext.Session.GetString(DS.SessionToken));

                if(response != null && response.IsSuccessful)
                {
                    TempData["successful"] = $"Villa \"{model.Name}\" successfully updated!";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveVilla(int villaId)
        {
            var response = await _villaService.Get<APIResponse>(villaId, HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsSuccessful)
            {
                VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveVilla(VillaDto model)
        {
            var response = await _villaService.Remove<APIResponse>(model.Id, HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsSuccessful)
            {
                TempData["successful"] = $"Villa successfully removed!";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = $"Villa could not be removed!";
            return View(model);
        }
    }
}
