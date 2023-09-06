using Matcher.Business.Interfaces;

namespace Matcher.Business.Core;

public class OffsetCachingKey : ICachingKey
{
    public string Prefix { get; }
    public string Value { get; }

    public OffsetCachingKey(int id)
    {
        Prefix = MatchingConstants.OffsetCachingPrefix + id;
        
        Value = Prefix;
    }
}
