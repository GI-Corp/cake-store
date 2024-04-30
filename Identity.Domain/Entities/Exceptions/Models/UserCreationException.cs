using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class UserCreationException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.UserCreationException;

    public UserCreationException()
    {
    }

    public UserCreationException(string message) : base(message)
    {
    }

    public UserCreationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}