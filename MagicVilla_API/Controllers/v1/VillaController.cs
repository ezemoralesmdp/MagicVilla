using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Models.Specifications;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Get all the villas");
                IEnumerable<Villa> villaList = await _villaRepo.GetAll();

                _response.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("VillasPaginated")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaPaginated([FromQuery] Parameters parameters)
        {
            try
            {
                var villaList = _villaRepo.GetAllPaginated(parameters);
                _response.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _response.statusCode = HttpStatusCode.OK;
                _response.TotalPages = villaList.MetaData.TotalPages;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                throw;
            }

            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError($"Error when bringing the villa with Id: {id}. Zero is not a valid ID.");
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = $"Error when bringing the villa with Id: {id}. Zero is not a valid ID.";
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.Get(x => x.Id == id);

                if (villa == null)
                {
                    _logger.LogError($"Error when bringing the villa with Id: {id}. This villa does not exist.");
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.SingleErrorMessage = $"Error when bringing the villa with Id: {id}. This villa does not exist.";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto villaCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = "The model sent is not valid";
                    return BadRequest(_response);
                }

                if (await _villaRepo.Get(x => x.Name.ToLower() == villaCreateDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", $"The villa with the name '{villaCreateDto.Name}' already exists.");
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = $"The villa with the name '{villaCreateDto.Name}' already exists.";
                    return BadRequest(_response);
                }

                if (villaCreateDto == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = "Villa model is null";
                    return BadRequest(_response);
                }

                Villa model = _mapper.Map<Villa>(villaCreateDto);
                model.DateInsert = DateTime.Now;
                model.DateUpdate = DateTime.Now;

                await _villaRepo.Create(model);
                _response.Result = model;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = $"Failed to delete village with Id: {id}. Zero is not a valid ID.";
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.Get(x => x.Id == id);

                if (villa == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.SingleErrorMessage = $"Failed to delete village with Id: {id}. This villa does not exist.";
                    return NotFound(_response);
                }

                await _villaRepo.Remove(villa);

                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.Id)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.SingleErrorMessage = $"Failed to update village with Id: {id}. This villa does not exist.";
                return BadRequest(_response);
            }

            Villa model = _mapper.Map<Villa>(villaUpdateDto);

            await _villaRepo.Update(model);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.SingleErrorMessage = $"Failed to update village with Id: {id}. Zero is not a valid ID.";
                return BadRequest(patchDto);
            }

            var villa = await _villaRepo.Get(x => x.Id == id, false);

            if (villa == null)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.NotFound;
                _response.SingleErrorMessage = $"Failed to update village with Id: {id}. This villa does not exist.";
                return NotFound(villa);
            }

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            if (!ModelState.IsValid)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.SingleErrorMessage = "The model sent is not valid";
                return BadRequest(ModelState);
            }

            patchDto.ApplyTo(villaDto, ModelState);

            Villa model = _mapper.Map<Villa>(villaDto);

            await _villaRepo.Update(model);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
