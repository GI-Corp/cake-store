using Shared.Presentation.ViewModels.Reference;

namespace CakeStore.Application.Interfaces;

public interface IErrorService
{
    string Get(short code, string languageId);
    short GetStatusCode(short code, string languageId);
    ErrorViewModel GetModel(short code, string languageId);
}