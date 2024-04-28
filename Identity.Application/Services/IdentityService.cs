using System.Security.Claims;
using AutoMapper;
using Identity.Application.Dto.Auth;
using Identity.Application.Dto.Identity;
using Identity.Application.Helpers;
using Identity.Application.Interfaces;
using Identity.Domain.Abstractions.Interfaces;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Exceptions.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Common.Logging.Interfaces;

namespace Identity.Application.Services;

public class IdentityService : IIdentityService
{
    private readonly IEventLoggerService<IdentityService> _logger;
    private readonly IIdentityRepository _identityRepository;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordHasher<AppUser> _hasher;
    private readonly IMapper _mapper;
    
    public IdentityService(IEventLoggerService<IdentityService> logger, IIdentityRepository identityRepository,
        RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _hasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<AppUserDto> CreateOrUpdateUserAsync(RegisterDto registerDto)
    {
        bool isValidModel = await ValidateRegistrationModelAsync(registerDto);

        if (!isValidModel)
            throw new InvalidRegistrationDataException();
        
        var userExists = await _identityRepository.ExistsByUserNameAsync(registerDto.UserName);

        if (userExists)
            throw new UserAlreadyExistsException();

        var createdUserDto = await CreateUserAsync(registerDto);

        if (createdUserDto == null)
            throw new UserCreationException();

        return createdUserDto;
    }
    
    public async Task<bool> ValidateRegistrationModelAsync(RegisterDto registerDto)
    {
        if (!LanguageValidation.ValidateLanguage(registerDto.LanguageId))
            throw new InvalidLanguageException();

        if (!registerDto.UserName.CheckForPhoneNumber())
            throw new PhoneNotValidException();
        
        if (!registerDto.Password.CheckPassword())
            throw new PasswordNotValidException();

        return true;
    }

    public async Task<AppUserDto> GetUserDetailsByIdAsync(Guid userId)
    {
        var user = await _identityRepository.FindByIdAsync(userId, includeReferences: true);

        if (user == null)
            throw new UserNotFoundException();

        return _mapper.Map<AppUserDto>(user);
    }

    private async Task<AppUserDto?> CreateUserAsync(RegisterDto registerDto)
    {
        var appUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = registerDto.UserName,
            PhoneNumber = registerDto.UserName,
            PhoneNumberConfirmed = false,
        };

        var userResult = await _userManager.CreateAsync(appUser, registerDto.Password);
        var roleResult = await _userManager.AddToRoleAsync(appUser, registerDto.Role);

        var claimsAdded = registerDto.Role switch
        {
            "Seller" => await AddSellerClaims(appUser),
            "Customer" => await AddCustomerClaims(appUser),
            _ => throw new InvalidRoleException()
        };

        if (!userResult.Succeeded || !roleResult.Succeeded || !claimsAdded)
            throw new UserCreationException($"Failed to add claims or role to a user {appUser.Id}.");

        var userProfile = await CreateUserProfileAsync(appUser, registerDto);
        var userSetting = await CreateUserSettingAsync(appUser, registerDto);

        if (userProfile == null || userSetting == null)
            throw new UserCreationException("Failed to create userProfile or userSetting entities.");
        
        // TODO: publish an event here, to do something
        return _mapper.Map<AppUserDto>(appUser);
    }

    private async Task<UserProfile?> CreateUserProfileAsync(AppUser appUser, RegisterDto registerDto)
    {
        var userProfile = new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = appUser.Id,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        var profileEntity = await _identityRepository.CreateUserProfileAsync(userProfile);

        if (profileEntity == null)
            throw new ProfileCreationException();

        return profileEntity;
    }
    
    private async Task<UserSetting?> CreateUserSettingAsync(AppUser appUser, RegisterDto registerDto)
    {
        var userSetting = new UserSetting
        {
            Id = Guid.NewGuid(),
            UserId = appUser.Id,
            LanguageId = registerDto.LanguageId,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        var settingEntity = await _identityRepository.CreateUserSettingAsync(userSetting);

        if (settingEntity == null)
            throw new SettingCreationException();

        return settingEntity;
    }

    private async Task<bool> AddCustomerClaims(AppUser appUser)
    {
        try
        {
            await _userManager.AddClaimAsync(appUser, new Claim("IdentityConfirmed", "True"));
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to add customer claims to user {appUser.Id}: {e.Message}");
            return false;
        }
        
        return true;
    }

    private async Task<bool> AddSellerClaims(AppUser appUser)
    {
        try
        {
            await _userManager.AddClaimAsync(appUser, new Claim("IdentityConfirmed", "True"));
            await _userManager.AddClaimAsync(appUser, new Claim("SellerStatusConfirmed", "True"));
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to add seller claims to user {appUser.Id}: {e.Message}");
            return false;
        }
        
        return true;
    }
}