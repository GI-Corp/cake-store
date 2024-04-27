using Identity.Domain.Abstractions.Interfaces;
using Identity.Domain.Entities.Auth;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.DAL.Repositories;

public class IdentityRepository : IIdentityRepository, IDisposable
{
    private readonly IdentityContext _context;
    private bool _disposed = false;

    public IdentityRepository(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistsByUserNameAsync(string userName)
    {
        return await _context.Users.AnyAsync(u => u.UserName!.Equals(userName));
    }

    public async Task<AppUser?> FindByNameAsync(string userName, bool includeReferences = false)
    {
        IQueryable<AppUser?> query = _context.Users.Where(u => u.UserName!.Equals(userName)).AsNoTracking();

        if (includeReferences)
        {
            query = query.Include(u => u.UserProfile)
                .Include(u => u.UserSetting)
                .ThenInclude(us => us.Language);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<AppUser?> FindByIdAsync(Guid userId, bool includeReferences = false)
    {
        IQueryable<AppUser?> query = _context.Users.Where(u => u.Id == userId).AsNoTracking();

        if (includeReferences)
        {
            query = query.Include(u => u.UserProfile)
                .Include(u => u.UserSetting)
                .ThenInclude(us => us.Language);
        }

        return await query.FirstOrDefaultAsync();    
    }

    public async Task<UserSetting?> FindUserSettingByIdAsync(Guid userId)
    {
        IQueryable<UserSetting?> query = _context.UserSettings.Where(u => u.UserId == userId).AsNoTracking();
        return await query.FirstOrDefaultAsync();
    }
        

    public async Task<UserProfile?> FindUserProfileByIdAsync(Guid userId)
    {
        IQueryable<UserProfile?> query = _context.UserProfiles.Where(u => u.UserId == userId).AsNoTracking();
        return await query.FirstOrDefaultAsync();
    }

    public async Task<AppUser?> CreateUserAsync(AppUser user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<UserProfile?> CreateUserProfileAsync(UserProfile profile)
    {
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile; 
    }

    public async Task<UserSetting?> CreateUserSettingAsync(UserSetting setting)
    {
        _context.UserSettings.Add(setting);
        await _context.SaveChangesAsync();
        return setting;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}