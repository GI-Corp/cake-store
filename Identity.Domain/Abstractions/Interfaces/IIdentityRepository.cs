using Identity.Domain.Entities.Auth;

namespace Identity.Domain.Abstractions.Interfaces;

public interface IIdentityRepository
{
    Task<bool> ExistsByUserNameAsync(string userName);
    Task<AppUser?> FindByNameAsync(string userName, bool includeReferences);
    Task<AppUser?> FindByIdAsync(Guid userId, bool includeReferences);
    Task<UserSetting?> FindUserSettingByIdAsync(Guid userId);
    Task<UserProfile?> FindUserProfileByIdAsync(Guid userId);
    Task<AppUser?> CreateUserAsync(AppUser user);
    Task<UserProfile?> CreateUserProfileAsync(UserProfile profile);
    Task<UserSetting?> CreateUserSettingAsync(UserSetting setting);


}