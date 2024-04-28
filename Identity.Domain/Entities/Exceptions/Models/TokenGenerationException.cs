using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class TokenGenerationException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.TokenGenerationException;

    public TokenGenerationException()
    {
    }

    public TokenGenerationException(string message) : base(message)
    {
    }

    public TokenGenerationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}