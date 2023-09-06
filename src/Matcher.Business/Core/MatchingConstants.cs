namespace Matcher.Business.Core;

public static class MatchingConstants
{
    public static readonly int ScoreDelta = 1;

    public static readonly int DefaultTake = 100;
    public static readonly int DefaultOffset = 0;

    public static readonly string OffsetCachingPrefix = "offset:";
    public static readonly TimeSpan OffsetCachingTimeSpan = TimeSpan.FromMinutes(10);

    public static readonly string ProfilesCachingPrefix = "profiles:";
    public static readonly TimeSpan ProfilesCachingTimeSpan = TimeSpan.FromMinutes(10);
}
