using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class IdentityUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> identityUserRoleConfiguration)
    {
        identityUserRoleConfiguration.ToTable("AppUserRoles", IdentityContext.IdentityScheme);
    }
}