using Microsoft.AspNetCore.Identity;
using Shared.Domain.Entities.Abstraction;

namespace Identity.Domain.Entities.Auth;

public class UserProfile : IBaseEntity<Guid>
{
    public Guid Id { get; set; }
    private Guid UserId { get; set; }
    public AppUser AppUser { get; set; }
    public DateTime? BirthDate { get; set; }

    [ProtectedPersonalData] 
    public string FirstName { get; set; }
    [ProtectedPersonalData] 
    public string LastName { get; set; }
    
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}