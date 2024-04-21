using CakeStoreApp.Mappers.Reference.ViewModels;

namespace Shared.Presentation.ViewModels.Reference;

public interface IReferenceContainer
{
    List<LanguageViewModel> Languages { get; set; }

    List<ErrorViewModel> Errors { get; set; }
}