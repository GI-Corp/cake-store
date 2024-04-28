using Identity.Application.Dto.Auth;
using Identity.Application.Dto.Identity;

namespace Identity.Application.Interfaces;

public interface IIdentityService
{
    Task<AppUserDto> CreateOrUpdateUserAsync(RegisterDto registerDto);
    Task<bool> ValidateRegistrationModelAsync(RegisterDto registerDto);
    Task<AppUserDto> GetUserDetailsByIdAsync(Guid userId);
}