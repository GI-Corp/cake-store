using CakeStore.Application.Exceptions.Constants;
using Shared.Domain.Entities.Exceptions;

namespace CakeStore.Application.Exceptions.Models;

public class InternalServerException : BusinessFlowException
{
    public override short Code => (short)ExceptionTypes.InternalServerException;

    public InternalServerException()
    {
    }

    public InternalServerException(string message) : base(message)
    {
    }

    public InternalServerException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}