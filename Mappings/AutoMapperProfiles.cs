using AutoMapper;
using Test1LearnNewVersion.Models.DTO;
using Test1LearnNewVersion.Models.Entities;

namespace Test1LearnNewVersion.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<userDTO, user>()
                .ReverseMap();

            CreateMap<AdduserRequestDTO, user>() .ReverseMap();
            CreateMap<UpdateuserRequestDTO, user>() .ReverseMap();
        }
    }
}
