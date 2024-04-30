using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class InvalidRoleException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.InvalidRoleException;

    public InvalidRoleException()
    {
    }

    public InvalidRoleException(string message) : base(message)
    {
    }

    public InvalidRoleException(string message, Exception innerException) : base(message, innerException)
    {
    }
}