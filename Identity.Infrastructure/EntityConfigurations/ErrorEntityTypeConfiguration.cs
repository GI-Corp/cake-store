using Identity.Domain.Entities.Auth;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class ErrorEntityTypeConfiguration: IEntityTypeConfiguration<Error>
{
    public void Configure(EntityTypeBuilder<Error> errorConfiguration)
    {
        errorConfiguration.ToTable(nameof(IdentityContext.Errors), IdentityContext.IdentityScheme);
        errorConfiguration.HasKey(p => new { p.Code, p.LanguageId });
        errorConfiguration.HasOne(w => w.Language).WithMany().HasForeignKey(w => w.LanguageId);
        errorConfiguration.Property(p => p.LanguageId).HasMaxLength(3).IsRequired();
        errorConfiguration.Property(w => w.Message).HasMaxLength(500).IsRequired();
        errorConfiguration.Property(w => w.HttpStatusCode).HasDefaultValue(0).IsRequired();
    }
}