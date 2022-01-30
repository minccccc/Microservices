using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult TestConnection(){
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound TestConnection");
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from CommandService");

            var platforms = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
        }

    }
}