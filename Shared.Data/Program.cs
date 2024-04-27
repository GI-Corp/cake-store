using Microsoft.EntityFrameworkCore;
using Shared.Data.DAL.Contexts;
using Shared.Data.Seeders;

namespace Shared.Data;

internal static class Program
{
    private static async Task Main()
    {
        const string connectionString =
            "Host=database;Port=5432;Username=cake;Password=9549df94-6753-418d-9ce9-5b3db74992a2;Database=CakeStoreDb;";

        await using var context = new SharedContext(new DbContextOptionsBuilder<SharedContext>()
            .UseNpgsql(connectionString).Options);
        var seeder = new SharedContextSeeder(context);

        Console.WriteLine("Seeding started. Please wait.");
        await seeder.SeedAsync();
        Console.WriteLine("Seeding completed! Press any key to exit...");

        Console.ReadKey();
    }
}