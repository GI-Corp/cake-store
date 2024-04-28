using Shared.Domain.Entities.Exceptions;

namespace Identity.Domain.Entities.Exceptions.Models;

public class SettingCreationException : BusinessFlowException
{
    public override short Code => (short)IdentityExceptions.SettingCreationException;

    public SettingCreationException()
    {
    }

    public SettingCreationException(string message) : base(message)
    {
    }

    public SettingCreationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}