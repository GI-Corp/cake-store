using System.ComponentModel.DataAnnotations;

namespace Shared.Presentation.ViewModels.Reference;

public class LanguageViewModel
{

    [MaxLength(3)]
    public string Id { get; set; }

    [MaxLength(20)]
    [Required]
    public string Name { get; set; }

    [MaxLength(70)]
    [Required]
    public string NativeName { get; set; }
}