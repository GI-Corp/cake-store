using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class IdentityUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> identityUserConfiguration)
    {
        identityUserConfiguration.ToTable("AppUserClaims", IdentityContext.IdentityScheme);
        identityUserConfiguration.Property(p => p.ClaimType).HasMaxLength(50);
        identityUserConfiguration.Property(p => p.ClaimValue).HasMaxLength(100);
    }
}