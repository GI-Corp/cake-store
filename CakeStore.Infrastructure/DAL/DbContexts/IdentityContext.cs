using CakeStore.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace CakeStore.Infrastructure.DAL.DbContexts;

public class IdentityContext : DbContext
{
    private const string IdentityScheme = "Identity";

    public IdentityContext(DbContextOptions<IdentityContext> contextOptions) : base(contextOptions)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(IdentityScheme);
        modelBuilder.Entity<AppUser>().ToTable(nameof(AppUsers)).HasKey(w => new { w.Id });
        modelBuilder.Entity<UserSession>().ToTable(nameof(UserSessions)).HasKey(w => new { w.Id });
        modelBuilder.Entity<UserProfile>().ToTable(nameof(UserProfiles)).HasKey(w => new { w.Id });
        
    }
}