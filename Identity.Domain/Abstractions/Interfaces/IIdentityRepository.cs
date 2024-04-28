using System.Collections.ObjectModel;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Session;
using Identity.Domain.Entities.Token;

namespace Identity.Domain.Abstractions.Interfaces;

public interface IIdentityRepository
{
    Task<bool> ExistsByUserNameAsync(string userName);
    Task<bool> ValidateUserPasswordAsync(Guid userId, string password);
    Task<AppUser?> FindByNameAsync(string userName, bool includeReferences);
    Task<AppUser?> FindByIdAsync(Guid userId, bool includeReferences);
    Task<UserSetting?> FindUserSettingByIdAsync(Guid userId);
    Task<UserProfile?> FindUserProfileByIdAsync(Guid userId);
    Task<AppUser?> CreateUserAsync(AppUser user);
    Task<bool> UpdateUserAsync(AppUser user);
    Task<UserProfile?> CreateUserProfileAsync(UserProfile profile);
    Task<UserSetting?> CreateUserSettingAsync(UserSetting setting);
    Task<UserSession?> CreateUserSessionAsync(UserSession session);
    Task<UserSession?> FindUserSessionByIdAsync(Guid userId, string? hostAddress);
    Task<bool> DeleteSessionAsync(UserSession session);
    Task<RefreshToken?> CreateRefreshTokenAsync(RefreshToken refreshToken);
    Task<List<RefreshToken>> GetRefreshTokensBySessionIdAsync(UserSession session);
    Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken);
}