using AutoMapper;
using Identity.Domain.Entities.Reference;
using Shared.Presentation.ViewModels.Reference;

namespace Identity.Presentation.Mappers.Identity;

public class LanguageProfile : Profile
{
    public LanguageProfile()
    {
        CreateMap<LanguageViewModel, Language>()
            .ReverseMap();
    }
}