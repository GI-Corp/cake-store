using Microsoft.EntityFrameworkCore;
using Shared.Data.DAL.Contexts;
using Shared.Domain.Entities.Reference;

namespace Shared.Data.Seeders;

public class SharedContextSeeder
{
    private readonly SharedContext _context;

    public SharedContextSeeder(SharedContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SeedAsync()
    {
        Console.WriteLine("Stared seeding languages.");
        await SeedLanguagesAsync();
        Console.WriteLine("Languages are seeded.");
    }

    private async Task SeedLanguagesAsync()
    {
        var fileName = Path.Combine(Directory.GetCurrentDirectory(), "Fixtures", "languages.csv");
        var languages = await File.ReadAllLinesAsync(fileName);
        var entities = languages.Select(w =>
        {
            var split = w.Split("|", StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 3)
                return new Language()
                {
                    Id = split[0].Trim(),
                    Name = split[1].Trim(),
                    DisplayName = split[2].Trim()
                }; 
            return null;
        }).Where(w => w != null);
        
        if (!await _context.Languages.AnyAsync())
        {
            _context.Languages.AddRange(entities!);

            if (await _context.SaveChangesAsync() <= 0)
                throw new InvalidOperationException("Failed to seed languages.");
        }
    }
}