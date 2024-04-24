using Identity.Application.Interfaces;
using Identity.Infrastructure.DAL.DbContexts;
using Shared.Presentation.ViewModels.Reference;

namespace Identity.Application.Services;

public class ErrorService : IErrorService
{
    private readonly IdentityContext _context;

    public ErrorService(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string Get(short code, string languageId)
    {
        var error = _context
                        .Errors
                        .FirstOrDefault(w => w.Code == code && w.LanguageId == languageId)
                    ?? throw new ArgumentNullException($"Error with code {code} is not found.");

        return error.Message;
    }
    
    public short GetStatusCode(short code, string languageId)
    {
        var error = _context
                        .Errors
                        .FirstOrDefault(w => w.Code == code && w.LanguageId == languageId)
                    ?? throw new ArgumentNullException($"Error with code {code} is not found.");

        return error.HttpStatusCode;
    }
    
    public ErrorViewModel GetModel(short code, string languageId)
    {
        var error = _context
                        .Errors
                        .FirstOrDefault(w => w.Code == code && w.LanguageId == languageId)
                    ?? throw new ArgumentNullException($"Error with code {code} is not found.");

        var errorViewModel = new ErrorViewModel
        {
            Code = error.Code,
            LanguageId = error.LanguageId,
            Message = error.Message,
            HttpStatusCode = error.HttpStatusCode
        };

        return errorViewModel;
    }
}