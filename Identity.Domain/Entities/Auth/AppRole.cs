using Microsoft.AspNetCore.Identity;
using Shared.Domain.Entities.Abstraction;

namespace Identity.Domain.Entities.Auth;

public class AppRole : IdentityRole<Guid>, IEntity<Guid>
{
}