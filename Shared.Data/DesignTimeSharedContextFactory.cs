using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Data.DAL.Contexts;

namespace Shared.Data;

public class DesignTimeSharedContextFactory : IDesignTimeDbContextFactory<SharedContext>
{
    public SharedContext CreateDbContext(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        const string configurationFile = "appsettings.json";
        const string databaseName = "CakeStoreDb";
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configurationFile)
            .Build();

        var builder = new DbContextOptionsBuilder<SharedContext>();
        var connectionString = configuration.GetConnectionString(databaseName);
        builder.UseNpgsql(connectionString);

        return new SharedContext(builder.Options);
    }
}