using System.ComponentModel.DataAnnotations;
using Shared.Common.Extensions.Attribute;
using Shared.Presentation.ViewModels.Reference;

namespace Identity.Application.Dto.Identity;

public class UserSettingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
    
    [Required]
    [AllowUpdate]
    [MaxLength(3)]
    public string LanguageId { get; set; }

    public LanguageViewModel Language { get; set; }
}