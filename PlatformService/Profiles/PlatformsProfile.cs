using AutoMapper;
using PlatformService.DTOs;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            //Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<PlatformCreateDTO, Platform>();
            CreateMap<PlatformReadDTO, PlatformPublishedDto>();
        }
    }
}