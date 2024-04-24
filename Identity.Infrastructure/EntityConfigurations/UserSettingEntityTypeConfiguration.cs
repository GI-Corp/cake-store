using Identity.Domain.Entities.Auth;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class UserSettingEntityTypeConfiguration : IEntityTypeConfiguration<UserSetting>
{
    public void Configure(EntityTypeBuilder<UserSetting> userSettingConfiguration)
    {
        userSettingConfiguration.ToTable(nameof(IdentityContext.UserSettings), IdentityContext.IdentityScheme);
        userSettingConfiguration.HasIndex(p => new { p.Id, p.UserId });
        userSettingConfiguration
            .HasOne(w => w.AppUser)
            .WithOne(w => w.UserSetting)
            .HasForeignKey<UserSetting>(w => w.UserId);
        userSettingConfiguration
            .HasOne(w => w.Language)
            .WithMany()
            .HasForeignKey(w => w.LanguageId)
            .IsRequired();
        userSettingConfiguration
            .Property(w => w.LanguageId)
            .HasMaxLength(3);
    }
}