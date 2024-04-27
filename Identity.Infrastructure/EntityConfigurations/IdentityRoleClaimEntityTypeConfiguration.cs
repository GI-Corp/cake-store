using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class IdentityRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> identityRoleConfiguration)
    {
        identityRoleConfiguration.ToTable("AppRoleClaims", IdentityContext.IdentityScheme);
        identityRoleConfiguration.Property(p => p.ClaimType).HasMaxLength(50);
        identityRoleConfiguration.Property(p => p.ClaimValue).HasMaxLength(100);
    }
}