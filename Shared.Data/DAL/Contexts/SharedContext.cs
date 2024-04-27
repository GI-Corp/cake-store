using Microsoft.EntityFrameworkCore;
using Shared.Data.EntityConfigurations;
using Shared.Domain.Entities.Reference;

namespace Shared.Data.DAL.Contexts;

public class SharedContext : DbContext
{
    private const string ReferenceScheme = "References";

    public SharedContext(DbContextOptions<SharedContext> options) : base(options)
    {
    }

    public DbSet<Language> Languages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(ReferenceScheme);
        modelBuilder.ApplyConfiguration(new LanguageEntityTypeConfiguration());
    }
}