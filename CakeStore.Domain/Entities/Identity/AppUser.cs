using Shared.Domain.Entities.Abstraction;

namespace CakeStore.Domain.Entities.Identity;

public class AppUser : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string UserName { get; private set; }
    public string PhoneNumber { get; private set; }
    public UserProfile UserProfile { get; private set; }
}