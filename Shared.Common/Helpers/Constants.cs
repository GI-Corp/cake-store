namespace Shared.Common.Helpers;

public static class Constants
{
    public static class JwtClaimIdentifiers
    {
        public const string Id = "id";
        public const string UserName = "UserName";
        public const string PhoneNumber = "PhoneNumber";
        public const string LanguageId = "LanguageId";
        public const string SessionId = "SessionId";
    }
    
    public static class Role
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Seller = "Seller";
    }
    
    public static class RegEx
    {
        public const string DigitsRegEx = "^\\d+$";
        public const string PhoneNumberRegEx = "^[+]*[(]{0,1}([0-9]{1,4})[)]{0,1}[-\\s\\./0-9]*$";
        public const string NonDigitsRegEx = "[^0-9]";
    }
    
    public static class Miscellaneous
    {
        public const string DefaultTimeZoneId = "UTC";
        public const string DefaultSystemLanguageId = "ru";
        public const string DefaultExceptionLanguageId = "en";
        public const string BasicAuthentication = "BasicAuthentication";
        public const string Basic = "Basic";
        public const string Bearer = "Bearer";
        public const string AdminUserPolicy = "AdminUserPolicy";
        public const string CustomerPolicy = "CustomerPolicy";
        public const string SellerPolicy = "SellerPolicy";
        public const int DefaultPageSize = 10;
    }
    
    public static class Languages
    {
        public const string uz = "uz";
        public const string ru = "ru";
        public const string en = "en";
    }
}