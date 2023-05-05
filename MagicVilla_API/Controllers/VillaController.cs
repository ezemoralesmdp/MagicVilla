using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Get all the villas");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto?> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError($"Error when bringing the villa with Id: {id}. The Id: {id} does not exist.");
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                _logger.LogError($"Error when bringing the villa with Id: {id}. There is no villa with the Id: {id}");
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_db.Villas.FirstOrDefault(x => x.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ExistingName", $"The villa with the name '{villaDto.Name}' already exists.");
                return BadRequest(ModelState);
            }

            if(villaDto == null)
                return BadRequest();

            if (villaDto.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            Villa model = new()
            {
                Name = villaDto.Name,
                Details = villaDto.Details,
                ImageUrl = villaDto.ImageUrl,
                Occupants = villaDto.Occupants,
                Fee = villaDto.Fee,
                SquareMeters = villaDto.SquareMeters,
                Amenity = villaDto.Amenity
            };

            _db.Villas.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
                return BadRequest();

            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if(villa == null)
                return NotFound();

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
                return BadRequest();

            Villa model = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Details = villaDto.Details,
                ImageUrl = villaDto.ImageUrl,
                Occupants = villaDto.Occupants,
                Fee = villaDto.Fee,
                SquareMeters = villaDto.SquareMeters,
                Amenity = villaDto.Amenity
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null || id == 0)
                return BadRequest();

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);

            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupants = villa.Occupants,
                Fee = villa.Fee,
                SquareMeters = villa.SquareMeters,
                Amenity = villa.Amenity
            };

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Villa model = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Details = villaDto.Details,
                ImageUrl = villaDto.ImageUrl,
                Occupants = villaDto.Occupants,
                Fee = villaDto.Fee,
                SquareMeters = villaDto.SquareMeters,
                Amenity = villaDto.Amenity
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
