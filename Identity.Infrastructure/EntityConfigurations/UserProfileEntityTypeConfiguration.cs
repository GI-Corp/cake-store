using Identity.Domain.Entities.Auth;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class UserProfileEntityTypeConfiguration: IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> userProfileConfiguration)
    {
        userProfileConfiguration.ToTable(nameof(IdentityContext.UserProfiles), IdentityContext.IdentityScheme);

        userProfileConfiguration.HasKey(p => new { p.Id });
        userProfileConfiguration.HasIndex(p => new { p.Id, p.UserId });
        userProfileConfiguration.Property(w => w.FirstName).HasMaxLength(50).IsRequired(false);
        userProfileConfiguration.Property(w => w.LastName).HasMaxLength(50).IsRequired(false);
        userProfileConfiguration.Property(w => w.BirthDate).IsRequired(false);
        userProfileConfiguration.HasOne(w => w.AppUser)
            .WithOne(w => w.UserProfile)
            .HasForeignKey<UserProfile>(w => w.UserId)
            .IsRequired();
    }
}