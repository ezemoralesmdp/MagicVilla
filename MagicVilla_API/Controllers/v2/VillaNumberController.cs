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

namespace MagicVilla_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
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

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2" };
        }
    }
}
