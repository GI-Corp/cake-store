namespace Shared.Domain.Entities.Exceptions;

public class BusinessFlowException : BaseException
{
    public BusinessFlowException()
    {
    }

    public BusinessFlowException(string message) : base(message)
    {
    }

    public BusinessFlowException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}