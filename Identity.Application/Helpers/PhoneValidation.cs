namespace Identity.Application.Helpers;

public static class PhoneValidation
{
    public static bool CheckForPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return false;
        
        phoneNumber = phoneNumber.Replace('+', ' ').Trim();
        return !phoneNumber.Any(char.IsLetter) && phoneNumber.Length == 12 && phoneNumber.StartsWith("998");
    }
}
