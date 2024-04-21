using Shared.Presentation.ViewModels.Reference;

namespace CakeStoreApp.Mappers.Reference;

public interface IReferenceContainer
{
    List<LanguageViewModel> Languages { get; set; }

    List<ErrorViewModel> Errors { get; set; }
}