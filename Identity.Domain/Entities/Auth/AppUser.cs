using Microsoft.AspNetCore.Identity;
using Shared.Domain.Entities.Abstraction;

namespace Identity.Domain.Entities.Auth;

public class AppUser : IdentityUser<Guid>, IEntity<Guid>
{
    public UserProfile UserProfile { get; set; }
    public UserSetting UserSetting { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime LastSignedInOn { get; set; } = DateTime.MinValue;
}