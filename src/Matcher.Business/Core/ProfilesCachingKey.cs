using Matcher.Business.Interfaces;

namespace Matcher.Business.Core;

public class ProfilesCachingKey : ICachingKey
{
    public string Prefix { get; }
    public string Value { get; }

    public ProfilesCachingKey(int id, ProfileCriteria criteria)
    {
        Prefix = MatchingConstants.ProfilesCachingPrefix + id;
        
        Value = Prefix + criteria.ToString();
    }
}
