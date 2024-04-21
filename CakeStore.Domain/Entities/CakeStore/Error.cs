using CakeStore.Domain.Entities.Reference;

namespace CakeStore.Domain.Entities.CakeStore;

public class Error : IEquatable<Error>
{
    /// <summary>
    ///  Gets or sets code number of error.
    /// </summary>
    public short Code { get; set; }

    /// <summary>
    ///  Gets or sets an instance of related <see cref="Reference.Language"/> class.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    ///  Gets or sets a unique identifier of related <see cref="Reference.Language"/> class.
    /// </summary>
    public string LanguageId { get; set;  }

    /// <summary>
    /// Gets or sets short description of the message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets Http Status Code for an error
    /// </summary>
    public short HttpStatusCode { get; set; }

    public bool Equals(Error? error)
    {
        if (error is null)
            return false;

        return LanguageId == error.LanguageId && Code == error.Code;
    }

    public override bool Equals(object? otherError)
    {
        return otherError is Error error && Equals(error);
    }

    public override int GetHashCode()
    {
        return LanguageId.GetHashCode() ^ Code.GetHashCode();
    }
}