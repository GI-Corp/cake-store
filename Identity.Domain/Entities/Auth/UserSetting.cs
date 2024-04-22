using CakeStore.Domain.Entities.Reference;
using Shared.Domain.Entities.Abstraction;

namespace Identity.Domain.Entities.Auth;

public class UserSetting : IBaseEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser AppUser { get; set; }
    public string LanguageId { get; set; }
    public Language Language { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}