using Identity.Domain.Entities.Auth;
using Identity.Infrastructure.DAL.DbContexts;

namespace Identity.Presentation.Helpers;

public class IdentityInitializer
{
    private readonly IdentityContext _context;

    public IdentityInitializer(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SeedAsync()
    {
        await SeedFixturesAsync();
        await SeedErrorsAsync();
    }

    private async Task SeedFixturesAsync()
    {
        var fileName = Path.Combine(Directory.GetCurrentDirectory(), "References", "fixtures.csv");
        await File.ReadAllBytesAsync(fileName);
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
                throw new InvalidOperationException($"Cannot seed available errors.");
            }
        }
    }
}