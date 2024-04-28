using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.Identity;

public class UserSession : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? HostAddress { get; set; }
    public short StatusId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}