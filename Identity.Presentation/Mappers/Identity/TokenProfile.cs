using AutoMapper;
using Identity.Application.Dto.Auth;
using Identity.Domain.Entities.Token;

namespace Identity.Presentation.Mappers.Identity;

public class TokenProfile : Profile
{
    public TokenProfile()
    {
        CreateMap<TokenDto, JwtToken>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Token.Login))
            .ForMember(dest => dest.AuthToken, opt => opt.MapFrom(src => src.Token.AuthToken))
            .ForMember(dest => dest.ExpiresIn, opt => opt.MapFrom(src => src.Token.ExpiresIn))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken));
        
        CreateMap<TokenDto, JwtToken>().ReverseMap();
    }
}