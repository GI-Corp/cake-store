using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class PhoneNotValidException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.PhoneNotValidException;

    public PhoneNotValidException()
    {
    }

    public PhoneNotValidException(string message) : base(message)
    {
    }

    public PhoneNotValidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}