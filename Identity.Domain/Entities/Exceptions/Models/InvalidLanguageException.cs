using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class InvalidLanguageException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.InvalidLanguageException;

    public InvalidLanguageException()
    {
    }

    public InvalidLanguageException(string message) : base(message)
    {
    }

    public InvalidLanguageException(string message, Exception innerException) : base(message, innerException)
    {
    }
}