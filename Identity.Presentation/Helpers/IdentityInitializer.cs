using System.Security.Claims;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Exceptions.Models;
using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Identity;
using Shared.Common.Helpers;

namespace Identity.Presentation.Helpers;

public class IdentityInitializer
{
    private readonly IdentityContext _context;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public IdentityInitializer(IdentityContext context, RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task SeedAsync()
    {
        await SeedFixturesAsync();
        await SeedErrorsAsync();
    }

    private async Task SeedFixturesAsync()
    {
        IdentityResult identityResult;

        // adding admin role to database
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            var adminRole = new AppRole { Name = "Admin" };
            identityResult = await _roleManager.CreateAsync(adminRole);

            if (!identityResult.Succeeded)
                throw new InvalidOperationException("Cannot create admin role");

            await _roleManager.AddClaimAsync(adminRole, new Claim("IsAdmin", "True"));
        }
        
        // adding customer role to database
        if (!await _roleManager.RoleExistsAsync("Customer"))
        {
            var customerRole = new AppRole { Name = "Customer" };
            identityResult = await _roleManager.CreateAsync(customerRole);

            if (!identityResult.Succeeded)
                throw new InvalidOperationException("Cannot create customer role");

            await _roleManager.AddClaimAsync(customerRole, new Claim("IdentityConfirmed", "True"));
        }
        
        // adding seller role to database
        if (!await _roleManager.RoleExistsAsync("Seller"))
        {
            var sellerRole = new AppRole { Name = "Seller" };
            identityResult = await _roleManager.CreateAsync(sellerRole);

            if (!identityResult.Succeeded)
                throw new InvalidOperationException("Cannot create seller role");

            await _roleManager.AddClaimAsync(sellerRole, new Claim("IdentityConfirmed", "True"));
            await _roleManager.AddClaimAsync(sellerRole, new Claim("SellerStatusConfirmed", "True"));

        }

        await SeedMockUsersAsync();
    }

    private async Task SeedMockUsersAsync()
    {
        var admin = await CreateUserAsync("998000000000", Constants.AppUserType.Admin);
        var customer = await CreateUserAsync("998001234455", Constants.AppUserType.Customer);
        var seller = await CreateUserAsync("998001234456", Constants.AppUserType.Seller);

        if (admin == null)
            throw new UserCreationException("Unable to find, create admin user.");

        if (customer == null)
            throw new UserCreationException("Unable to find, create customer user.");
        
        if (seller == null)
            throw new UserCreationException("Unable to find, create seller user.");
    }
    
    private async Task SeedErrorsAsync()
    {
        var fileName = Path.Combine(Directory.GetCurrentDirectory(), "References", "errors.csv");
        var errors = await File.ReadAllLinesAsync(fileName);
        var entities = errors.Select(w =>
        {
            var split = w.Split("|", StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 4)
            {
                return new Error
                {
                    Code = Convert.ToInt16(split[0].Trim()),
                    LanguageId = split[1].Trim(),
                    HttpStatusCode = Convert.ToInt16(split[2].Trim()),
                    Message = split[3].Trim()
                };
            } return null;
        }).Where(w => w != null);

        var errorsFromDb = _context.Errors
            .AsEnumerable();

        var newErrors = entities.Except(errorsFromDb);

        IEnumerable<Error?> enumerable = newErrors.ToList();
        if (enumerable.Any())
        {
            _context.Errors.AddRange(enumerable!);

            if (await _context.SaveChangesAsync() <= 0)
            {
                throw new InvalidOperationException("Cannot seed available errors.");
            }
        }
    }

    private async Task<AppUser?> CreateUserAsync(string userName, string userType)
    {
        if (!Constants.AppUserType.AppUserTypes.Contains(userType))
            return null;

        var user = await _userManager.FindByNameAsync(userName);

        if (user != null)
            return user;
        
        IdentityResult claimsResult;
        bool isSuccess;

        switch (userType)
        {
            case Constants.AppUserType.Admin:
                var adminUser = new AppUser
                {
                    PhoneNumberConfirmed = true,
                    PhoneNumber = "998000000000",
                    Email = "gicorp.work@gmail.com",
                    EmailConfirmed = true,
                    UserName = "998000000000",
                };
                var isAdminCreated = await _userManager.CreateAsync(adminUser, "654321aAxdda@");
                var isAdminAddedToRole = await _userManager.AddToRoleAsync(adminUser, "Admin");

                var adminClaims = new[]
                {
                    new Claim("IdentityConfirmed", "True"),
                    new Claim("SellerStatusConfirmed", "True")
                };

                claimsResult = await _userManager.AddClaimsAsync(adminUser, adminClaims);

                if (!isAdminCreated.Succeeded || !isAdminAddedToRole.Succeeded || !claimsResult.Succeeded)
                    throw new UserCreationException("An error occured while creating admin user.");
                
                var adminProfile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = adminUser.Id,
                    FirstName = "Islom",
                    LastName = "Gulomkodirov",
                    BirthDate = new DateTime(1998, 12, 30),
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.UserProfiles.Add(adminProfile);
                isSuccess = await _context.SaveChangesAsync() > 0;

                if (!isSuccess)
                    throw new UserCreationException("An error occured while creating profile the admin user.");

                var adminSetting = new UserSetting
                {
                    Id = Guid.NewGuid(),
                    UserId = adminUser.Id,
                    LanguageId = Constants.Languages.ru,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.UserSettings.Add(adminSetting);
                isSuccess = await _context.SaveChangesAsync() > 0;

                if (!isSuccess)
                    throw new UserCreationException("An error occured while creating settings the admin user.");

                return adminUser;

            case Constants.AppUserType.Customer:
                var customer = new AppUser
                {
                    PhoneNumberConfirmed = true,
                    PhoneNumber = "998001234455",
                    Email = "customer.dotnet@gmail.com",
                    EmailConfirmed = true,
                    UserName = "998001234455",
                };

                var isCustomerCreated = await _userManager.CreateAsync(customer, "123456aA@");
                var isCustomerAddedToRole = await _userManager.AddToRoleAsync(customer, "Customer");

                var customerClaims = new[] { new Claim("IdentityConfirmed", "True")};

                claimsResult = await _userManager.AddClaimsAsync(customer, customerClaims);

                if (!isCustomerCreated.Succeeded || !isCustomerAddedToRole.Succeeded || !claimsResult.Succeeded)
                    throw new UserCreationException("An error occured while creating customer user.");
                
                var customerProfile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = customer.Id,
                    FirstName = "Josh",
                    LastName = "Novikov",
                    BirthDate = new DateTime(1995, 10, 25),
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.UserProfiles.Add(customerProfile);
                isSuccess = await _context.SaveChangesAsync() > 0;

                if (!isSuccess)
                    throw new UserCreationException("An error occured while creating profile the customer user.");

                var customerSetting = new UserSetting
                {
                    Id = Guid.NewGuid(),
                    UserId = customer.Id,
                    LanguageId = Constants.Languages.ru,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.UserSettings.Add(customerSetting);
                isSuccess = await _context.SaveChangesAsync() > 0;

                if (!isSuccess)
                    throw new UserCreationException("An error occured while creating settings the customer user.");

                return customer;
            
            case Constants.AppUserType.Seller:
                var seller = new AppUser
                {
                    PhoneNumberConfirmed = true,
                    PhoneNumber = "998001234456",
                    Email = "seller.dotnet@gmail.com",
                    EmailConfirmed = true,
                    UserName = "998001234456",
                };

                var isSellerCreated = await _userManager.CreateAsync(seller, "1234567aA@");
                var isSellerAddedToRole = await _userManager.AddToRoleAsync(seller, "Seller");

                var sellerClaims = new[]
                {
                    new Claim("IdentityConfirmed", "True"),
                    new Claim("SellerStatusConfirmed", "True")
                };
                claimsResult = await _userManager.AddClaimsAsync(seller, sellerClaims);

                if (!isSellerCreated.Succeeded || !isSellerAddedToRole.Succeeded || !claimsResult.Succeeded)
                    throw new UserCreationException("An error occured while creating seller user.");
                
                var sellerProfile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = seller.Id,
                    FirstName = "Ramiz",
                    LastName = "Durov",
                    BirthDate = new DateTime(1970, 11, 02),
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.UserProfiles.Add(sellerProfile);
                isSuccess = await _context.SaveChangesAsync() > 0;

                if (!isSuccess)
                    throw new UserCreationException("An error occured while creating profile the seller user.");

                var sellerSetting = new UserSetting
                {
                    Id = Guid.NewGuid(),
                    UserId = seller.Id,
                    LanguageId = Constants.Languages.ru,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.UserSettings.Add(sellerSetting);
                isSuccess = await _context.SaveChangesAsync() > 0;

                if (!isSuccess)
                    throw new UserCreationException("An error occured while creating settings the seller user.");

                return seller;

            default: return null;
        }
    }
}