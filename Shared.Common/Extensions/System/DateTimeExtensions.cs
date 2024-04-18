namespace Shared.Common.Extensions.System;

public static class DateTimeExtensions
{
    private static readonly Lazy<DateTime> LazyUnixEpoch = new(() => DateTime.UnixEpoch);

    public static bool Expired(this DateTime source)
    {
        var target = DateTime.UtcNow;

        return source.CompareTo(target) < 0;
    }

    public static bool Expired(this DateTime source, TimeSpan duration)
    {
        return source < DateTime.UtcNow - duration;
    }

    public static ulong ToUnixTime(this DateTime source)
    {
        return (ulong)source.Subtract(LazyUnixEpoch.Value).TotalMilliseconds;
    }

    public static DateTime ToDateTime(this ulong milliseconds)
    {
        return LazyUnixEpoch.Value.AddMilliseconds(milliseconds).ToLocalTime();
    }

    public static DateTime ToLocal(this DateTime source)
    {
        return TimeZoneInfo.ConvertTime(source, TimeZoneInfo.Local);
    }

    public static DateTime LastDateOfMonth(this DateTime target)
    {
        return target.FirstDateOfMonth().AddMonths(1).AddDays(-1);
    }

    public static DateTime FirstDateOfMonth(this DateTime target)
    {
        return new DateTime(target.Year, target.Month, 1);
    }

    public static int GetLastDayOfMonth(this DateTime target)
    {
        return DateTime.DaysInMonth(target.Year, target.Month);
    }
}