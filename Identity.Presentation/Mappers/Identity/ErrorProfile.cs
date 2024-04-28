using AutoMapper;
using Identity.Domain.Entities.Auth;
using Shared.Presentation.ViewModels.Reference;

namespace Identity.Presentation.Mappers.Identity;

public class ErrorProfile : Profile
{
    public ErrorProfile()
    {
        CreateMap<ErrorViewModel, Error>()
            .ForMember(d => d.Language, opt => opt.Ignore())
            .ReverseMap();
    }
}