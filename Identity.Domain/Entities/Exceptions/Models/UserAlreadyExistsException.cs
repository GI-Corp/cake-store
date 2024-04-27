using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class UserAlreadyExistsException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.UserAlreadyExistsException;

    public UserAlreadyExistsException()
    {
    }

    public UserAlreadyExistsException(string message) : base(message)
    {
    }

    public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}