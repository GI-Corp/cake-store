using System.Reflection;
using System.Security.Authentication;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CakeStoreApp.Extensions;
using CakeStoreApp.Helpers;
using CakeStoreApp.Middlewares;
using Serilog;
using Shared.Common.Extensions.Swagger;

namespace CakeStoreApp;

public class Startup
{
    private IConfiguration _configuration { get; }
    public ILifetimeScope AutofacContainer { get; private set; }

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddCustomMvc()
            .AddSingleton(_configuration)
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .AddCustomDbContexts(_configuration)
            .AddServices()
            .AddOtlp(_configuration)
            .AddCustomAuthentication(_configuration)
            .AddCustomAuthorization()
            .AddCustomApiVersioning()
            .AddCustomOptions(_configuration)
            .AddIntegrationServices(_configuration)
            .AddSwaggerDocumentation(Path.Combine(AppContext.BaseDirectory, "CakeStore.Presentation.xml"))
            .AddHttpClient();

        serviceCollection.AddHttpClient("HttpBaseClient")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls13
            });

        var container = new ContainerBuilder();
        container.Populate(serviceCollection);
    }

    public void Configure(
        IApplicationBuilder application, IWebHostEnvironment environment,
        CakeStoreInitializer cakeStoreInitializer, ReferenceContainerInitializer referenceContainerInitializer,
        IHostApplicationLifetime applicationLifetime)
    {
        AutofacContainer = application.ApplicationServices.GetAutofacRoot();

        applicationLifetime.ApplicationStarted.Register(OnApplicationStarted);
        applicationLifetime.ApplicationStopped.Register(OnApplicationStopped);

        if (environment.IsDevelopment())
            application.UseDeveloperExceptionPage();

        application.UseSwaggerDocs();
        application.UseAuthentication();
        application.UseRouting();
        application.UseAuthorization();
        application.UseMiddleware<CakeStoreExceptionHandlerMiddleware>();
        application.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        cakeStoreInitializer.SeedAsync().Wait();
        referenceContainerInitializer.InitializeReferencesAsync().Wait();
    }
    
    public void OnApplicationStarted()
    {
        Log.Information($"{Assembly.GetExecutingAssembly().GetName().Name} - Started");
    }

    public void OnApplicationStopped()
    {
        Log.Information($"{Assembly.GetExecutingAssembly().GetName().Name} - Stopped");
        Log.CloseAndFlush();
    }
}