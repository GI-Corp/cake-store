using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CakeStore.Application.Interfaces;
using CakeStore.Application.Services;
using CakeStore.Infrastructure.DAL.DbContexts;
using CakeStoreApp.Helpers;
using CakeStoreApp.Mappers.Reference;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared.Common.Authorization.Encryption;
using Shared.Common.Authorization.JWT;
using Shared.Common.Encryption.Interfaces;
using Shared.Common.Encryption.Specifications;
using Shared.Common.Helpers;
using Shared.Common.Logging.Interfaces;
using Shared.Common.Logging.Specifications;
using Shared.Data.Abstraction;
using Shared.Data.DAL.Repository;

namespace CakeStoreApp.Extensions;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection AddCustomMvc(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddControllers(action =>
        {
            action.ReturnHttpNotAcceptable = true;
        }).AddNewtonsoftJson(setupAction =>
        {
            setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            setupAction.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            setupAction.SerializerSettings.Converters.Add(new DecimalJsonConverter());
            
        }).AddXmlDataContractSerializerFormatters()
        
            .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = "http://localhost:8010/errors/",
                    Title = "One or more model validation errors occured.",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = "See the errors property for details.",
                    Instance = context.HttpContext.Request.Path
                };

                problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };

            };
        });
        
        return serviceCollection;
    }

    public static IServiceCollection AddCustomDbContexts(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<CakeStoreContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CakeStoreDb")));
        
        serviceCollection.AddDbContext<ReferenceContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CakeStoreDb")));

        serviceCollection.AddScoped<DbContext, CakeStoreContext>();
        serviceCollection.AddScoped<DbContext, ReferenceContext>();

        return serviceCollection;
    }

    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton(typeof(IEventLoggerService<>), typeof(EventLoggerService<>))
            .AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>))
            
            .AddTransient<CakeStoreInitializer>()
            .AddTransient<ReferenceContainerInitializer>()
            
            .AddSingleton<IReferenceContainer, ReferenceContainer>()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddSingleton<IEncryptionService, EncryptionService>()

            .AddExternalApis()
            .AddCakeStoreAdapters()
            
            .AddSingleton<IErrorService, ErrorService>()
    
            .AddSingleton<JsonSerializerSettings>(cfg => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

        return serviceCollection;
    }
    
    public static IServiceCollection AddOtlp(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddOpenTelemetry()
            .WithTracing(builder => builder
                    .AddSource("CakeStore")
                    .ConfigureResource(resource => resource.AddService("CakeStore"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .SetSampler(new AlwaysOnSampler())
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(configuration.GetValue<string>("JaegerUrl")!);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    }));

        return serviceCollection;
    }
    
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtIssuerOptions));
        var secretKey = jwtOptions[nameof(JwtIssuerOptions.SecretKey)];
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey!));
        
        serviceCollection.Configure<JwtIssuerOptions>(options =>
        {
            options.Issuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)]!;
            options.Audience = jwtOptions[nameof(JwtIssuerOptions.Audience)]!;
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)],
            ValidAudience = jwtOptions[nameof(JwtIssuerOptions.Audience)],

            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = "role",
            NameClaimType = "name",

        };

        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(configureOptions =>
        {
            configureOptions.ClaimsIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
            configureOptions.TokenValidationParameters = tokenValidationParameters;
            configureOptions.SaveToken = true;
        });

        return serviceCollection;
    }
    
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("AdminUserPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("CustomerPolicy", policy =>
            {
                policy.RequireRole("Customer");
                policy.RequireClaim("IdentityConfirmed", "True" );
            });
        });

        return serviceCollection;
    }
    
    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(cfg =>
        {
            cfg.DefaultApiVersion = new ApiVersion(1, 0);
            cfg.AssumeDefaultVersionWhenUnspecified = true;
            cfg.ReportApiVersions = true;
        });

        return serviceCollection;
    }
    
    public static IServiceCollection AddIntegrationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection;
    }
    
    public static IServiceCollection AddCustomOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .Configure<EncryptionOptions>(configuration.GetSection(nameof(EncryptionOptions)));

        return serviceCollection;
    }
    
    private static IServiceCollection AddExternalApis(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
    
    private static IServiceCollection AddCakeStoreAdapters(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
    
    
}