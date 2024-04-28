using AutoMapper;
using Identity.Application.Dto.Identity;
using Identity.Domain.Entities.Auth;

namespace Identity.Presentation.Mappers.Identity;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<AppUserDto, AppUser>()
            .ForMember(d => d.UserProfile, opt => opt.MapFrom(s => s.UserProfile))
            .ForMember(d => d.UserSetting, opt => opt.MapFrom(s => s.UserSetting))
            .ReverseMap();

        CreateMap<UserProfileDto, UserProfile>().ReverseMap();
        CreateMap<UserSettingDto, UserSetting>().ReverseMap();
    }
}
