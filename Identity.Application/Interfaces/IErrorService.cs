using Shared.Presentation.ViewModels.Reference;

namespace Identity.Application.Interfaces;

public interface IErrorService
{
    string Get(short code, string languageId);
    short GetStatusCode(short code, string languageId);
    ErrorViewModel GetModel(short code, string languageId);
}