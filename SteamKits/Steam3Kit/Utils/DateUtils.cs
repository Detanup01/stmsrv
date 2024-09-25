namespace Steam3Kit.Utils;

/// <summary>
/// Contains various utility functions for dealing with dates.
/// </summary>
public static class DateUtils
{
    /// <summary>
    /// Converts a given unix timestamp to a DateTime
    /// </summary>
    /// <param name="unixTime">A unix timestamp expressed as seconds since the unix epoch</param>
    /// <returns>DateTime representation</returns>
    public static DateTime DateTimeFromUnixTime(ulong unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds((long)unixTime).UtcDateTime;
    }
    /// <summary>
    /// Converts a given DateTime into a unix timestamp representing seconds since the unix epoch.
    /// </summary>
    /// <param name="time">DateTime to be expressed</param>
    /// <returns>64-bit wide representation</returns>
    public static ulong DateTimeToUnixTime(DateTime time)
    {
        return (ulong)new DateTimeOffset(time).ToUnixTimeSeconds();
    }
}
