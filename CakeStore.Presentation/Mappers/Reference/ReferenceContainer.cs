using Shared.Presentation.ViewModels.Reference;

namespace CakeStoreApp.Mappers.Reference
{
    public class ReferenceContainer : IReferenceContainer
    {
        public List<LanguageViewModel> Languages { get; set; } = new();

        public List<ErrorViewModel> Errors { get; set; } = new();
    }
}
