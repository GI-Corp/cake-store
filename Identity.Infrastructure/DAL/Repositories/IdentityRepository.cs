using System.Collections.ObjectModel;
using Identity.Domain.Abstractions.Interfaces;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Session;
using Identity.Domain.Entities.Token;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.DAL.Repositories;

public class IdentityRepository : IIdentityRepository, IDisposable
{
    private readonly IdentityContext _context;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private bool _disposed = false;

    public IdentityRepository(IdentityContext context, IPasswordHasher<AppUser> passwordHasher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<bool> ExistsByUserNameAsync(string userName)
    {
        return await _context.Users.AnyAsync(u => u.UserName!.Equals(userName));
    }

    public async Task<bool> ValidateUserPasswordAsync(Guid userId, string password)
    {
        var user = await FindByIdAsync(userId);
        var verificationResult = _passwordHasher.VerifyHashedPassword(user!, user.PasswordHash, password);
        return verificationResult == PasswordVerificationResult.Success;
    }

    public async Task<AppUser?> FindByNameAsync(string userName, bool includeReferences = false)
    {
        bool userExists = await _context.Users.AnyAsync(u => u.UserName == userName);
    
        if (!userExists)
            return null;
        
        IQueryable<AppUser?> query = _context.Users.Where(u => u.UserName!.Equals(userName)).AsNoTracking();

        if (includeReferences)
        {
            query = query.Include(u => u.UserProfile)
                .Include(u => u.UserSetting);
            // .ThenInclude(us => us.Language);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<AppUser?> FindByIdAsync(Guid userId, bool includeReferences = false)
    {
        IQueryable<AppUser?> query = _context.Users.Where(u => u.Id == userId).AsNoTracking();

        if (includeReferences)
        {
            query = query.Include(u => u.UserProfile)
                .Include(u => u.UserSetting);
            // .ThenInclude(us => us.Language);
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

    public async Task<bool> UpdateUserAsync(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
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

    public async Task<UserSession?> CreateUserSessionAsync(UserSession session)
    {
        _context.UserSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<UserSession?> FindUserSessionByIdAsync(Guid userId, string? hostAddress)
    {
        IQueryable<UserSession?> query = _context.UserSessions.Where(u => 
            u.UserId == userId && u.HostAddress == hostAddress);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteSessionAsync(UserSession session)
    {
        _context.UserSessions.Remove(session);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<RefreshToken?> CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<List<RefreshToken>> GetRefreshTokensBySessionIdAsync(UserSession session)
    {
        var tokens = await _context.RefreshTokens
            .Where(u => u.SessionId == session.Id)
            .ToListAsync();

        return tokens;
    }
    
    public async Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        _context.Entry(refreshToken).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
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