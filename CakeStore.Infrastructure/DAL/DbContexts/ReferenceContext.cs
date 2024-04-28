using CakeStore.Domain.Entities.Reference;
using Microsoft.EntityFrameworkCore;

namespace CakeStore.Infrastructure.DAL.DbContexts;

public class ReferenceContext : DbContext
{
    private const string ReferenceScheme = "References";

    public ReferenceContext(DbContextOptions<ReferenceContext> contextOptions) : base(contextOptions)
    {
    }
    
    public DbSet<Language> Languages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(ReferenceScheme);
        modelBuilder.Entity<Language>().ToTable(nameof(Languages)).HasKey(w => new { w.Id });

    }
}