using System.Text.RegularExpressions;

namespace Identity.Application.Helpers;

public static class PasswordValidation
{
    public static bool CheckPassword(this string password)
    {
        // Between 8 and 20 characters long.
        // At least one uppercase letter.
        // At least one lowercase letter.
        // At least one digit.
        // Contains only Latin characters (letters and digits).
        // Does not contain any spaces.
        // Contains at least one special character from the defined set of special characters.
        const string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+{}[\]:;,.<>?])[A-Za-z\d!@#$%^&*()_+{}[\]:;,.<>?]{8,20}$";
        return Regex.IsMatch(password, pattern) && !Regex.IsMatch(password, @"\s");
    }
}