using Shared.Presentation.ViewModels.Reference;

namespace Identity.Presentation.Mappers.Reference;

public interface IReferenceContainer
{
    List<LanguageViewModel> Languages { get; set; }

    List<ErrorViewModel> Errors { get; set; }
}