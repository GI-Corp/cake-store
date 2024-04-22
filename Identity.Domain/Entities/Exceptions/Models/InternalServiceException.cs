using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class InternalServiceException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.InternalServiceException;

    public InternalServiceException()
    {
    }

    public InternalServiceException(string message) : base(message)
    {
    }

    public InternalServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}