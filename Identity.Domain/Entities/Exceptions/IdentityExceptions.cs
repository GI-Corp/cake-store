namespace Identity.Domain.Entities.Exceptions;

public enum IdentityExceptions : short
{
    NotSet = 0,
    InternalServiceException = -10000,
    PhoneIsNotValidException = -10001,
}