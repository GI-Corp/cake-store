using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class PhoneIsNotValidException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.PhoneIsNotValidException;

    public PhoneIsNotValidException()
    {
    }

    public PhoneIsNotValidException(string message) : base(message)
    {
    }

    public PhoneIsNotValidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}