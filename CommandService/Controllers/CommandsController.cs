using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDTO>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Get Commands for Platform {platformId}");

            if (!_repository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDTO> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Get Command {commandId} for Platform {platformId}");

            if (!_repository.PlatformExist(platformId))
            {
                return NotFound();
            }
            var command = _repository.GetCommand(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDTO>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommandForPlatform(int platformId, CommandCreateDTO commandDto)
        {
            Console.WriteLine($"--> CreateCommandForPlatform {platformId}");

            if (!_repository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);
            
            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDTO>(command);
            return CreatedAtRoute(
                nameof(GetCommandForPlatform), 
                new {platformId = platformId, commandId = commandReadDto.Id},
                commandReadDto
                );
        }

    }
}