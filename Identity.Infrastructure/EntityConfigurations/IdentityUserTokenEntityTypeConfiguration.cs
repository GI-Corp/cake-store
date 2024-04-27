using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class IdentityUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> identityUserTokenConfiguration)
    {
        identityUserTokenConfiguration.ToTable("AppUserTokens", IdentityContext.IdentityScheme);
        identityUserTokenConfiguration.Property(p => p.Name).HasMaxLength(200);
        identityUserTokenConfiguration.Property(p => p.Value).HasMaxLength(1000);
        identityUserTokenConfiguration.Property(p => p.LoginProvider).HasMaxLength(50);
    }
}