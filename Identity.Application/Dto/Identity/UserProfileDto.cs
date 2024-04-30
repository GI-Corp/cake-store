using System.ComponentModel.DataAnnotations;
using Shared.Common.Extensions.Attribute;

namespace Identity.Application.Dto.Identity;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
    
    [MaxLength(50)]
    [AllowUpdate]
    public string FirstName { get; set; }

    [MaxLength(50)]
    [AllowUpdate]
    public string LastName { get; set; }

    [AllowUpdate]
    public DateTime? BirthDate { get; set; }
    
}