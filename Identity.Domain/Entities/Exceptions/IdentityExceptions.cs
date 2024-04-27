namespace Identity.Domain.Entities.Exceptions;

public enum IdentityExceptions : short
{
    NotSet = 0,
    InternalServiceException = -10000,
    PhoneIsNotValidException = -10001,
    UserCreationException = -10002,
    InvalidRegistrationDataException = -10003,
    UserAlreadyExistsException = -10004,
    InvalidRoleException = -10005,
    ProfileCreationException = -10006,
    SettingCreationException = -10007,
    InvalidLanguageException = -10008,
    PhoneNotValidException = -10009,
    PasswordNotValidException = -10010
}