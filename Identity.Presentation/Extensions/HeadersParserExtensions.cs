using Identity.Presentation.Mappers.Reference;
using Shared.Common.Helpers;

namespace Identity.Presentation.Extensions;

public static class HeadersParserExtensions
{
    private static string GetDefaultLanguageId(this string languageId, IReferenceContainer referenceContainer)
    {
        if (string.IsNullOrWhiteSpace(languageId))
            return Constants.Miscellaneous.DefaultExceptionLanguageId;

        var language = referenceContainer.Languages.FirstOrDefault(l => l.Id == languageId);
        
        return language != null ? language.Id : Constants.Miscellaneous.DefaultExceptionLanguageId;
    }

    public static string GetLanguageIdFromHeaders(this HttpRequest httpRequest, IReferenceContainer referenceContainer)
    {
        var languageId = httpRequest.Headers["Accept-Language"].ToString();
        return GetDefaultLanguageId(languageId, referenceContainer);
    }
    
    public static string GetHostAddress(this HttpRequest httpRequest)
    {
        return httpRequest.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.0";
    }
}