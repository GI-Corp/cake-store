using Autofac.Extensions.DependencyInjection;
using CakeStoreApp;
using Serilog;

try
{
    var programEnvironment = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
    
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddEnvironmentVariables()
        .AddJsonFile(programEnvironment, optional: true, reloadOnChange: true)
        .Build();
    
    Serilog.Debugging.SelfLog.Enable(Console.Out);

    var loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration);

    Log.Logger = loggerConfiguration.CreateLogger();
    
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);

    var host = Host
        .CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseKestrel(options =>
                {
                    options.Configure(configuration.GetSection("Kestrel"));
                    options.UseSystemd();
                })
                .UseStartup<Startup>();
        })
        .UseSerilog()
        .Build();
    
    host.Run();
    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Program terminated unexpectedly.");
    Log.CloseAndFlush();
    return 1;
}