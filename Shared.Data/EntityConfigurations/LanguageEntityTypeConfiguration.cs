using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Data.DAL.Contexts;
using Shared.Domain.Entities.Reference;

namespace Shared.Data.EntityConfigurations;

internal class LanguageEntityTypeConfiguration
    : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> languageConfiguration)
    {
        languageConfiguration.ToTable(nameof(SharedContext.Languages));
        languageConfiguration.HasKey(p => new { p.Id });
        languageConfiguration.Property(p => p.Id).HasMaxLength(3).IsRequired();
        languageConfiguration.Property(p => p.Name).HasMaxLength(20).IsRequired();
        languageConfiguration.Property(p => p.DisplayName).HasMaxLength(70).IsRequired();
    }
}