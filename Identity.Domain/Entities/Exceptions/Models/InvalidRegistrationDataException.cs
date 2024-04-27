using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class InvalidRegistrationDataException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.InvalidRegistrationDataException;

    public InvalidRegistrationDataException()
    {
    }

    public InvalidRegistrationDataException(string message) : base(message)
    {
    }

    public InvalidRegistrationDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}