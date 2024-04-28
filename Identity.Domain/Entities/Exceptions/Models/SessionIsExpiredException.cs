using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class SessionIsExpiredException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.SessionIsExpiredException;

    public SessionIsExpiredException()
    {
    }

    public SessionIsExpiredException(string message) : base(message)
    {
    }

    public SessionIsExpiredException(string message, Exception innerException) : base(message, innerException)
    {
    }
}