namespace Shared.Domain.Entities.Exceptions;

public class BaseException : Exception
{
    public virtual short Code => default;
    public string Details { get; set; }
    
    public BaseException()
    {
    }

    public BaseException(string message) : base(message)
    {
        Details = message;
    }

    public BaseException(string message, Exception innerException) : base(message,
        innerException)
    {
        Details = message;
    }
}