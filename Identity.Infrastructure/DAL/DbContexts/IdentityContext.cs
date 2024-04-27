using Identity.Domain.Constants;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Session;
using Identity.Domain.Entities.Token;
using Identity.Infrastructure.EntityConfigurations;
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
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<IdentityUserRole<Guid>> IdentityUserRoles { get; set; }
    public DbSet<Error> Errors { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(IdentityScheme);
        builder.ApplyConfiguration(new AppUserEntityTypeConfiguration());
        builder.ApplyConfiguration(new AppRoleEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserSettingEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserProfileEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserSettingEntityTypeConfiguration());
        builder.ApplyConfiguration(new ErrorEntityTypeConfiguration());
        builder.ApplyConfiguration(new IdentityRoleClaimEntityTypeConfiguration());
        builder.ApplyConfiguration(new IdentityUserClaimEntityTypeConfiguration());
        builder.ApplyConfiguration(new IdentityUserLoginEntityTypeConfiguration());
        builder.ApplyConfiguration(new IdentityUserRoleEntityTypeConfiguration());
        builder.ApplyConfiguration(new IdentityUserTokenEntityTypeConfiguration());
    }
}