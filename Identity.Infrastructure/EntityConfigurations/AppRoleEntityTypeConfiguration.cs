using Identity.Domain.Entities.Auth;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class AppRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> appRoleConfiguration)
    {
        appRoleConfiguration.ToTable(nameof(IdentityContext.AppRoles), IdentityContext.IdentityScheme);
        appRoleConfiguration.Property(p => p.Name).HasMaxLength(50);
        appRoleConfiguration.Property(p => p.NormalizedName).HasMaxLength(50);
        appRoleConfiguration.Property(p => p.ConcurrencyStamp).HasMaxLength(200);
    }
}