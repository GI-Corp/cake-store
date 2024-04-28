using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using Identity.Application.Dto.Auth;
using Identity.Application.Dto.Identity;
using Identity.Application.Extensions;
using Identity.Application.Helpers;
using Identity.Application.Interfaces;
using Identity.Domain.Abstractions.Interfaces;
using Identity.Domain.Constants;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Exceptions.Models;
using Identity.Domain.Entities.Session;
using Identity.Domain.Entities.Token;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using Shared.Common.Authorization.JWT;
using Shared.Common.Authorization.JWT.Interfaces;
using Shared.Common.Helpers;
using Shared.Common.Logging.Interfaces;
using Shared.Data.Abstraction;

namespace Identity.Application.Services;

public class IdentityService : IIdentityService
{
    private readonly IEventLoggerService<IdentityService> _logger;
    private readonly IIdentityRepository _identityRepository;
    private IRepository<UserSession, Guid, IdentityContext> _sessionRepository;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtFactory _jwtFactory;
    private readonly JwtIssuerOptions _jwtOptions;
    private readonly IMapper _mapper;
    
    public IdentityService(IEventLoggerService<IdentityService> logger, IIdentityRepository identityRepository,
        RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,
        IMapper mapper, IRepository<UserSession, Guid, IdentityContext> sessionRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtFactory = jwtFactory ?? throw new ArgumentNullException(nameof(jwtFactory));
        _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
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

    public async Task<TokenDto> AuthorizeUserAsync(LoginDto loginDto, string? hostAddress)
    {
        var user = await _identityRepository.FindByNameAsync(loginDto.UserName, includeReferences: true);

        if (user == null)
            throw new UserNotFoundException();

        var isPasswordValid = await _identityRepository.ValidateUserPasswordAsync(user.Id, password: loginDto.Password);

        if (!isPasswordValid)
            throw new PasswordNotValidException();

        var userSession = await CreateOrGetUserSessionAsync(user, hostAddress);

        var identityClaims = await GetClaimsIdentity(user, sessionId: userSession.Id);

        var jwtToken = await GenerateJwtTokenAsync(user, identityClaims, userSession);

        if (jwtToken == null)
            throw new TokenGenerationException();
        
        return new TokenDto
        {
            SessionId = userSession.Id,
            Token = jwtToken
        };
    }

    private async Task<JwtToken> GenerateJwtTokenAsync(AppUser appUser, ClaimsIdentity identityClaims, UserSession session)
    {
        var jwtToken = await TokenGeneratorExtension.GenerateJwtToken(
            identityClaims,
            _jwtFactory,
            appUser.UserName!,
            _jwtOptions,
            new JsonSerializerSettings { Formatting = Formatting.Indented });

        appUser.LastSignedInOn = DateTime.Now;
        jwtToken.RefreshToken = await GenerateRefreshTokenAsync();
        
        var refreshToken = GetRefreshToken(jwtToken.RefreshToken, appUser.Id, session.Id);
        bool deactivated = await DeactivateOldRefreshTokensAsync(session);
        
        if (deactivated)
            _logger.LogInformation($"Refresh tokens for session {session.Id} are deactivated.");
        
        await _identityRepository.CreateRefreshTokenAsync(refreshToken);
        await _identityRepository.UpdateUserAsync(appUser);
        
        // TODO: Send email notification to user about login
        return jwtToken;
    }

    private async Task<bool> DeactivateOldRefreshTokensAsync(UserSession userSession)
    {
        var refreshTokens = await _identityRepository.GetRefreshTokensBySessionIdAsync(userSession);

        await Task.WhenAll(refreshTokens.Select(async token =>
        {
            token.IsActive = false;
            token.DeletedAt = DateTime.Now;

            await _identityRepository.UpdateRefreshTokenAsync(token);
        }));

        return true;
    }
    private static Task<string> GenerateRefreshTokenAsync()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Task.FromResult(Convert.ToBase64String(randomBytes));
    }
    
    private static RefreshToken GetRefreshToken(string token, Guid userId, Guid sessionId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            UserId = userId,
            SessionId = sessionId,
            IsActive = true,
            ExpiresAt = DateTime.Now.AddDays(10),
            CreatedAt = DateTime.Now
        };
    }

    private async Task<UserSession> CreateOrGetUserSessionAsync(AppUser appUser, string? hostAddress)
    {
        var userSession = await _identityRepository.FindUserSessionByIdAsync(appUser.Id, hostAddress);

        if (userSession == null)
        {
            var session = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = appUser.Id,
                HostAddress = hostAddress,
                StatusId = (int)SessionStatusTypes.Active,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _sessionRepository.CreateAsync(session);
            return session;
        }

        if (userSession.StatusId != (int)SessionStatusTypes.Active)
        {
            // TODO: Handle with oAuth2.0 bu verification code
            _logger.LogWarning($"User session {userSession.Id} has expired, needs to be activated.");
            
            await _identityRepository.DeleteSessionAsync(userSession);
            throw new SessionIsExpiredException();
        }

        return userSession;
    }
    
    private async Task<ClaimsIdentity> GetClaimsIdentity(AppUser user, Guid sessionId)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(Constants.JwtClaimIdentifiers.Id, user.Id.ToString()),
                new(Constants.JwtClaimIdentifiers.PhoneNumber, user.PhoneNumber!),
                new(Constants.JwtClaimIdentifiers.UserName, user.UserName!),
                new(Constants.JwtClaimIdentifiers.LanguageId, user.UserSetting.LanguageId),
                new(Constants.JwtClaimIdentifiers.SessionId, sessionId.ToString())
            }
            .Union(userClaims)
            .Union(roles.Select(w => new Claim(ClaimTypes.Role, w)));

        return _jwtFactory.GenerateClaimsIdentity(user.UserName!, claims); 
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