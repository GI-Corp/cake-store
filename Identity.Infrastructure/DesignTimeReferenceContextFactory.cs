﻿using Identity.Infrastructure.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure;

public class DesignTimeReferenceContextFactory : IDesignTimeDbContextFactory<ReferenceContext>
{
    public ReferenceContext CreateDbContext(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        const string configurationFile = "appsettings.json";
        const string databaseName = "IdentityDb";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configurationFile)
            .Build();

        var builder = new DbContextOptionsBuilder<ReferenceContext>();
        var connectionString = configuration.GetConnectionString(databaseName);
        builder.UseNpgsql(connectionString);

        return new ReferenceContext(builder.Options);
    }
}