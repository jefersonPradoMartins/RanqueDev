using AutoMapper;
using RanqueDev.Services.Dto;
using RanqueDev.Services.Dto.Autentication;
using RanqueDev.Domain.Entities;
using RanqueDev.Domain.Entities.Identity;

namespace RanqueDev.Api.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Question, CreateQuestao>().ReverseMap();
            CreateMap<Tag, CreateTagDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserReturnDto>().ReverseMap();
        }
    }
}



