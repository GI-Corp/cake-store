using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Session;
using Shared.Domain.Entities.Abstraction;

namespace Identity.Domain.Entities.Token;

public class RefreshToken : IBaseEntity<Guid>
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public AppUser AppUser { get; set; }
    public Guid SessionId { get; set; }
    public UserSession UserSession { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}