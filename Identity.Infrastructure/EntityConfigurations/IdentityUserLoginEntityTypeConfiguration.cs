using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class IdentityUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> identityUserLoginConfiguration)
    {
        identityUserLoginConfiguration.ToTable("AppUserLogins", IdentityContext.IdentityScheme);
        identityUserLoginConfiguration.Property(p => p.LoginProvider).HasMaxLength(50);
        identityUserLoginConfiguration.Property(p => p.ProviderDisplayName).HasMaxLength(100);
        identityUserLoginConfiguration.Property(p => p.ProviderKey).HasMaxLength(500);
    }
}