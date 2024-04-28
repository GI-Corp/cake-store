using CakeStore.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CakeStore.Infrastructure;

public class DesignTimeCakeStoreContextFactory : IDesignTimeDbContextFactory<CakeStoreContext>
{
    public CakeStoreContext CreateDbContext(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        const string configurationFile = "appsettings.json";
        const string databaseName = "CakeStoreDb";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configurationFile)
            .Build();

        var builder = new DbContextOptionsBuilder<CakeStoreContext>();
        var connectionString = configuration.GetConnectionString(databaseName);
        builder.UseNpgsql(connectionString);

        return new CakeStoreContext(builder.Options);
    }
}