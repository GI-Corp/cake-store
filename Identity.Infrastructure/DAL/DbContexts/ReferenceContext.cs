using CakeStore.Domain.Entities.Reference;
using Identity.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Identity.Infrastructure.DAL.DbContexts;

public class ReferenceContext : DbContext
{
    internal const string ReferenceScheme = "References";

    public ReferenceContext(DbContextOptions<ReferenceContext> options) : base(options)
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<BehaviorTypes>();
    }

    public DbSet<Language> Languages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(ReferenceScheme);
        builder.Entity<Language>().ToTable(nameof(Languages)).HasKey(w => new { w.Id });
    }
}