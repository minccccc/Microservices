using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<CommandCreateDTO, Command>();
            CreateMap<Command, CommandReadDTO>();
        }
    }
}