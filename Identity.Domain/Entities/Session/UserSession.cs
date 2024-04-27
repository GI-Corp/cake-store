using Identity.Domain.Entities.Auth;
using Shared.Domain.Entities.Abstraction;

namespace Identity.Domain.Entities.Session;

public class UserSession : IBaseEntity<Guid>, IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser AppUser { get; set; }
    public string? HostAddress { get; set; }
    public short StatusId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}