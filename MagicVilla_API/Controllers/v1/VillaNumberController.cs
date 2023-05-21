using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace MagicVilla_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaNumberController : ControllerBase
    {
        private readonly ILogger<VillaNumberController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly IVillaNumberRepository _villaNumberRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberController(
            ILogger<VillaNumberController> logger,
            IVillaRepository villaRepo,
            IVillaNumberRepository villaNumberRepo,
            IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _villaNumberRepo = villaNumberRepo;
            _mapper = mapper;
            _response = new();
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumberVillas()
        {
            try
            {
                _logger.LogInformation("Get all the villa numbers");
                IEnumerable<VillaNumber> villaNumberList = await _villaNumberRepo.GetAll(propertiesInclude: "Villa");

                _response.Result = _mapper.Map<IEnumerable<VillaNumberDto>>(villaNumberList);
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

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError($"Error when bringing the villa number with Id: {id}. Zero is not a valid ID.");
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = $"Error when bringing the villa number with Id: {id}. Zero is not a valid ID.";
                    return BadRequest(_response);
                }

                var villaNumber = await _villaNumberRepo.Get(x => x.VillaNo == id, propertiesInclude: "Villa");

                if (villaNumber == null)
                {
                    _logger.LogError($"Error when bringing the villa with Id: {id}. This villa does not exist.");
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.SingleErrorMessage = $"Error when bringing the villa with Id: {id}. This villa does not exist.";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto villaNumberCreateDto)
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

                if (await _villaNumberRepo.Get(x => x.VillaNo == villaNumberCreateDto.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", $"The villa number with the number '{villaNumberCreateDto.VillaNo}' already exists.");
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = $"The villa number with the number '{villaNumberCreateDto.VillaNo}' already exists.";
                    return BadRequest(_response);
                }

                if (await _villaRepo.Get(x => x.Id == villaNumberCreateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", $"The villa Id with the number '{villaNumberCreateDto.VillaId}' doesn't exists.");
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = $"The villa Id with the number '{villaNumberCreateDto.VillaId}' doesn't exists.";
                    return BadRequest(_response);
                }

                if (villaNumberCreateDto == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = "Villa model is null";
                    return BadRequest(_response);
                }

                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberCreateDto);
                model.DateInsert = DateTime.Now;
                model.DateUpdate = DateTime.Now;

                await _villaNumberRepo.Create(model);
                _response.Result = model;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { id = model.VillaNo }, _response);
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
        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.SingleErrorMessage = $"Failed to delete village number with Id: {id}. Zero is not a valid ID.";
                    return BadRequest(_response);
                }

                var villaNumber = await _villaNumberRepo.Get(x => x.VillaNo == id);

                if (villaNumber == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.SingleErrorMessage = $"Failed to delete village number with Id: {id}. This villa does not exist.";
                    return NotFound(_response);
                }

                await _villaNumberRepo.Remove(villaNumber);

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto villaNumberUpdateDto)
        {
            if (villaNumberUpdateDto == null || id != villaNumberUpdateDto.VillaNo)
            {
                _response.IsSuccessful = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.SingleErrorMessage = $"Failed to update village number with Id: {id}. This villa does not exist.";
                return BadRequest(_response);
            }

            if (await _villaRepo.Get(x => x.Id == villaNumberUpdateDto.VillaId) == null)
            {
                ModelState.AddModelError("ErrorMessages", $"The villa Id with the number '{villaNumberUpdateDto.VillaId}' doesn't exists.");
                return BadRequest(ModelState);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberUpdateDto);

            await _villaNumberRepo.Update(model);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
