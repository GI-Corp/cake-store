namespace Shared.Presentation.ViewModels.Exceptions;

public class ErrorResponseViewModel
{
    /// <summary>
    ///     Gets or sets error code.
    /// </summary>
    public short Code { get; set; }

    /// <summary>
    ///     Gets or sets languageId.
    /// </summary>
    public string LanguageId { get; set; }

    /// <summary>
    ///     Gets or sets error details.
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    ///     Gets or sets timestamp.
    /// </summary>
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}