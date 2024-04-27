using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class ProfileCreationException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.ProfileCreationException;

    public ProfileCreationException()
    {
    }

    public ProfileCreationException(string message) : base(message)
    {
    }

    public ProfileCreationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}