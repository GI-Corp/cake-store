using CakeStore.Domain.Entities.CakeStore;
using CakeStore.Domain.Entities.CakeStore.Cake;
using CakeStore.Domain.Entities.CakeStore.Media;
using CakeStore.Domain.Entities.CakeStore.Social;
using CakeStore.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CakeStore.Infrastructure.DAL.DbContexts;

public class CakeStoreContext : DbContext
{
    internal const string CakeStoreScheme = "CakeStore";

    public CakeStoreContext(DbContextOptions<CakeStoreContext> contextOptions) : base(contextOptions)
    {
    }
    
    public DbSet<Cake> Cakes { get; set; }
    public DbSet<CakeToUser> CakeToUsers { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Error> Errors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(CakeStoreScheme);
        modelBuilder.ApplyConfiguration(new CakeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CakeToUserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ErrorEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ImageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReactionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewEntityTypeConfiguration());
    }
}