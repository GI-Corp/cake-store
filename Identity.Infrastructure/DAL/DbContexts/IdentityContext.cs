using CakeStore.Domain.Entities.CakeStore;
using Identity.Domain.Constants;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Session;
using Identity.Domain.Entities.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Identity.Infrastructure.DAL.DbContexts;

public class IdentityContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    internal const string IdentityScheme = "Identity";

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<BehaviorTypes>();
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
    public DbSet<Error> Errors { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<IdentityUserRole<Guid>> IdentityUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(IdentityScheme);
    }
}