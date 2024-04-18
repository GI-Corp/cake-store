using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Shared.Domain.Entities.Swagger;

namespace Shared.Common.Extensions.Swagger;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, string xmlPath)
    {
        SwaggerOption options;
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            services.Configure<SwaggerOption>(configuration!.GetSection("swagger"));
            options = configuration.GetOptions<SwaggerOption>("swagger");
        }

        if (!options.Enabled) 
            return services;

        return services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc(options.Name,
                new OpenApiInfo
                {
                    Title = options.Title, 
                    Version = options.Version,
                    Description = options.Description
                });

            swaggerGenOptions.IncludeXmlComments(xmlPath);


            if (!options.IncludeSecurity)
                return;

            switch (options.SecurityType)
            {
                case "Bearer":
                    swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                    swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                    break;

                case "Basic":
                    swaggerGenOptions.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                    {
                        Description = "Basic auth added to authorization header",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "basic",
                        Type = SecuritySchemeType.Http
                    });
                    swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
                            },
                            new List<string>()
                        }
                    });
                    break;
            }
        });
    }

    public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
    {
        var options = builder!.ApplicationServices!.GetService<IConfiguration>()
            .GetOptions<SwaggerOption>("swagger");
        if (!options.Enabled) 
            return builder;

        var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "swagger" : options.RoutePrefix;

        builder.UseStaticFiles()
            .UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

        return options.ReDocEnabled
            ? builder.UseReDoc(c =>
            {
                c.RoutePrefix = routePrefix;
                c.SpecUrl = $"{options.Name}/swagger.json";
            })
            : builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
                c.RoutePrefix = routePrefix;
                c.DisplayRequestDuration();
            });
    }
    
    private static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(section).Bind(model);

        return model;
    }
}