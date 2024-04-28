using Identity.Domain.Entities.Token;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refTokenConfiguration)
    {
        refTokenConfiguration.ToTable(nameof(IdentityContext.RefreshTokens), IdentityContext.IdentityScheme);
        refTokenConfiguration.HasKey(p => new { p.Id });
        refTokenConfiguration.HasIndex(p => new { p.Id, p.UserId, p.SessionId });
        
        refTokenConfiguration
            .HasOne(h => h.AppUser)
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        refTokenConfiguration
            .HasOne(h => h.UserSession)
            .WithMany()
            .HasForeignKey(h => h.SessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}