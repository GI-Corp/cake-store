using CakeStore.Application.Exceptions.Constants;
using Shared.Domain.Entities.Exceptions;

namespace CakeStore.Application.Exceptions.Models;

public class CakeIsNotFoundException : BusinessFlowException
{
    public override short Code => (short)ExceptionTypes.CakeIsNotFoundException;

    public CakeIsNotFoundException()
    {
    }

    public CakeIsNotFoundException(string message) : base(message)
    {
    }

    public CakeIsNotFoundException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}