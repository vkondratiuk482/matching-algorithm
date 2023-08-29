namespace Matcher.Business.Core;

public static class MatchingConstants
{
    public static readonly int ScoreDelta = 1;

    public static readonly int DefaultTake = 100;
    public static readonly int DefaultOffset = 0;

    public static readonly string OffsetCachePrefix = "offset:";
    public static readonly TimeSpan OffsetCacheTimeSpan = TimeSpan.FromMinutes(10);

    public static readonly string ProfilesCachePrefix = "profiles:";
    public static readonly TimeSpan ProfilesCacheTimeSpan = TimeSpan.FromMinutes(10);
}
