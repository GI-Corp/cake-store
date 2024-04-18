namespace Shared.Common.Extensions.System;

public static class GuidExtensions
{
    public static string Encode(this Guid guid)
    {
        return Convert.ToBase64String(guid.ToByteArray());
    }

    public static Guid Decode(this string id)
    {
        var backId = Convert.FromBase64String(id);
        return new Guid(backId);
    }
}