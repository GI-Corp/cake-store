using System.ComponentModel.DataAnnotations;
using Identity.Domain.Entities.Exceptions.Models;

namespace Identity.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method |
                AttributeTargets.Parameter)]
public class PhoneNumbersAttribute : ValidationAttribute
{
    private const string AdditionalPhoneNumberCharacters = "-.()";
    private const string ExtensionAbbreviationExtDot = "ext.";
    private const string ExtensionAbbreviationExt = "ext";
    private const string ExtensionAbbreviationX = "x";


    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (value is not string valueAsString)
            throw new PhoneIsNotValidException();

        valueAsString = valueAsString.Replace("+", string.Empty).TrimEnd();

        valueAsString = RemoveExtension(valueAsString);

        var digitFound = false;

        foreach (var _ in from char c in valueAsString
                 where char.IsDigit(c)
                 select new { })
            digitFound = true;

        if (!digitFound)
            throw new PhoneIsNotValidException();

        var count = 0;
        foreach (var c in valueAsString)
        {
            if (!(char.IsDigit(c) || char.IsWhiteSpace(c) || AdditionalPhoneNumberCharacters.IndexOf(c) != -1))
                throw new PhoneIsNotValidException();

            count++;
        }

        if (count is < 12 or > 12)
            throw new PhoneIsNotValidException();

        return true;
    }

    private static string RemoveExtension(string potentialPhoneNumber)
    {
        var lastIndexOfExtension =
            potentialPhoneNumber.LastIndexOf(ExtensionAbbreviationExtDot, StringComparison.OrdinalIgnoreCase);
        if (lastIndexOfExtension >= 0)
        {
            var extension = potentialPhoneNumber.Substring(
                lastIndexOfExtension + ExtensionAbbreviationExtDot.Length);
            if (MatchesExtension(extension)) return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
        }

        lastIndexOfExtension =
            potentialPhoneNumber.LastIndexOf(ExtensionAbbreviationExt, StringComparison.OrdinalIgnoreCase);

        if (lastIndexOfExtension >= 0)
        {
            var extension = potentialPhoneNumber[(lastIndexOfExtension + ExtensionAbbreviationExt.Length)..];

            if (MatchesExtension(extension)) return potentialPhoneNumber[..lastIndexOfExtension];
        }

        lastIndexOfExtension =
            potentialPhoneNumber.LastIndexOf(ExtensionAbbreviationX, StringComparison.OrdinalIgnoreCase);

        if (lastIndexOfExtension >= 0)
        {
            var extension = potentialPhoneNumber[(lastIndexOfExtension + ExtensionAbbreviationX.Length)..];
            if (MatchesExtension(extension)) return potentialPhoneNumber[..lastIndexOfExtension];
        }

        return potentialPhoneNumber;
    }

    private static bool MatchesExtension(string potentialExtension)
    {
        potentialExtension = potentialExtension.TrimStart();
        if (potentialExtension.Length == 0) return false;

        foreach (var c in potentialExtension)
            if (!char.IsDigit(c))
                return false;

        return true;
    }
}