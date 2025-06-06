using AutoMapper;
using ControleDeAcesso.Model;
using ControleDeAcesso.DTOS;

namespace ControleDeAcesso.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginRequestDto, AuthModel>().ReverseMap();
            CreateMap<AuthModel, LoginResponseDto>();
            CreateMap<RegisterRequestDto, AuthModel>().ReverseMap();
            CreateMap<AuthModel, RegisterResponseDto>();
            CreateMap<Verify2FARequestDto, AuthModel>();
            CreateMap<AuthModel, Verify2FAResponseDto>();
        }
    }
}
