using Identity.Domain.Entities.Session;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class UserSessionEntityTypeConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> userSessionConfiguration)
    {
        userSessionConfiguration.ToTable(nameof(IdentityContext.UserSessions), IdentityContext.IdentityScheme);
        userSessionConfiguration.HasIndex(p => new { p.Id, p.UserId });
        
        userSessionConfiguration.HasOne(e => e.AppUser)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        userSessionConfiguration.Property(e => e.HostAddress).HasMaxLength(255);
        userSessionConfiguration.Property(e => e.StatusId).IsRequired();
    }
}