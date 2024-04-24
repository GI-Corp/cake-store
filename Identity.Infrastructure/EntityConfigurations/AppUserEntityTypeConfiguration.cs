using Identity.Domain.Entities.Auth;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> appUserConfiguration)
    {
        appUserConfiguration.ToTable(nameof(IdentityContext.AppUsers), IdentityContext.IdentityScheme);

        appUserConfiguration.HasKey(u => new { u.Id });
        appUserConfiguration.HasIndex(p => new { p.UserName }).IsUnique();
        appUserConfiguration.HasIndex(p => new { p.Email }).IsUnique();
        appUserConfiguration.HasIndex(p => new { p.PhoneNumber }).IsUnique();
        appUserConfiguration.Property(u => u.UserName).HasMaxLength(50);
        appUserConfiguration.Property(u => u.NormalizedUserName).HasMaxLength(100);
        appUserConfiguration.Property(u => u.PhoneNumber).HasMaxLength(50);
        appUserConfiguration.Property(u => u.Email).HasMaxLength(100);
        appUserConfiguration.Property(u => u.NormalizedEmail).HasMaxLength(100);
        appUserConfiguration.Property(u => u.AccessFailedCount);    
    }
}