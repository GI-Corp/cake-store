using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.Identity;

public class UserProfile : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}