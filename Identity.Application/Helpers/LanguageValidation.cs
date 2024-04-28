using Shared.Common.Helpers;

namespace Identity.Application.Helpers;

public static class LanguageValidation
{
    public static bool ValidateLanguage(string languageId)
    {
        if (string.IsNullOrEmpty(languageId))
            return false;
        
        return languageId is Constants.Languages.uz or Constants.Languages.ru or Constants.Languages.en;
    }
}
