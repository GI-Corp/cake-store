using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class PasswordNotValidException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.PasswordNotValidException;

    public PasswordNotValidException()
    {
    }

    public PasswordNotValidException(string message) : base(message)
    {
    }

    public PasswordNotValidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}