using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using PlatformService;

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
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(
                    dest => dest.ExternalID,
                    opt => opt.MapFrom(src => src.Id)
                );
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(desc => desc.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(desc => desc.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(desc => desc.Commands, opt => opt.Ignore());
        }
    }
}