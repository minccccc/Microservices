using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(
            IPlatformRepo repository, 
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");

            var platforms = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platform = _repository.GetPlatformById(id);
            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platform));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO dto)
        {
            var newPlatform = _mapper.Map<Platform>(dto);

            _repository.CreatePlatform(newPlatform);
            _repository.SaveChanges();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(newPlatform);
            
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDTO);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }
    }
}