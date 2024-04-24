using Identity.Domain.Entities.Reference;

namespace Identity.Domain.Entities.Auth;

public class Error : IEquatable<Error>
{
    public short Code { get; set; }
    public string LanguageId { get; set; }
    public Language Language { get; set; }
    public string Message { get; set; }
    public short HttpStatusCode { get; set; }

    public bool Equals(Error? other)
    {
        if (other is null)
            return false;

        return LanguageId == other.LanguageId && Code == other.Code;
    }

    public override bool Equals(object? other)
    {
        var error = other as Error;
        return error != null && Equals(error);
    }

    public override int GetHashCode()
    {
        return LanguageId.GetHashCode() ^ Code.GetHashCode();
    }
}