using Matcher.Business.Core;
using Matcher.Business.Interfaces;

namespace Matcher.Business.Services;

public class MatchService
{
    private readonly ICacheService _cacheService;
    private readonly ProfileService _profileService;

    public MatchService(ICacheService cacheService, ProfileService profileService)
    {
        _cacheService = cacheService;
        _profileService = profileService;
    }

    public async Task<Profile?> GetAsync(int userId, MatchingCriteria matchingCriteria)
    {
        var profile = await _profileService.GetByUserIdAsync(userId);
        var profileCriteria = new ProfileCriteria
        {
            Age = matchingCriteria.Age,
            Gender = matchingCriteria.Gender,
            MinScore = profile.Score - MatchingConstants.ScoreDelta,
            MaxScore = profile.Score + MatchingConstants.ScoreDelta,
        };

        var offsetCachingKey = new OffsetCachingKey(profile.Id);
        var profilesCachingKey = new ProfilesCachingKey(profile.Id, profileCriteria);

        var existingKey = await _cacheService.GetKeyByPatternAsync(profilesCachingKey.Prefix + "*");

        if (existingKey != profilesCachingKey.Value)
        {
            await Task.WhenAll(ResetOffsetAsync(offsetCachingKey), DeleteCachedProfilesAsync(profilesCachingKey));
        }

        var empty = await _cacheService.ListEmptyAsync(profilesCachingKey.Value);

        if (!empty)
        {
            return await _cacheService.PopFromListAsync<Profile>(profilesCachingKey.Value);
        }

        var offset = await GetOffsetAsync(offsetCachingKey);

        if (offset != MatchingConstants.DefaultOffset)
        {
            offset += MatchingConstants.DefaultTake;
        }

        await CommitOffsetAsync(offsetCachingKey, offset);

        var profiles = await _profileService.GetAsync(profileCriteria, MatchingConstants.DefaultTake, offset);

        if (profiles.Any())
        {
            await CacheProfilesAsync(profilesCachingKey, profiles);

            return await PopCachedProfileAsync(profilesCachingKey);
        }

        if (offset > 0)
        {
            await ResetOffsetAsync(offsetCachingKey);
        }

        return null;
    }

    private async Task CacheProfilesAsync(ICachingKey cachingKey, IEnumerable<Profile> profiles)
    {
        await _cacheService.CreateListAsync(cachingKey.Value, profiles, MatchingConstants.ProfilesCachingTimeSpan);
    }

    private async Task DeleteCachedProfilesAsync(ICachingKey cachingKey)
    {
        var key = await _cacheService.GetKeyByPatternAsync(cachingKey.Prefix + "*");

        await _cacheService.DeleteKeyAsync(key);
    }

    private async Task<Profile> PopCachedProfileAsync(ICachingKey cachingKey)
    {
        return await _cacheService.PopFromListAsync<Profile>(cachingKey.Value);
    }

    private async Task<int> GetOffsetAsync(ICachingKey cachingKey)
    {
        var value = await _cacheService.GetStringByKeyAsync(cachingKey.Value);

        return value == null ? MatchingConstants.DefaultOffset : int.Parse(value);
    }

    private async Task CommitOffsetAsync(ICachingKey cachingKey, int offset)
    {
        await _cacheService.SetStringByKeyAsync(cachingKey.Value, offset.ToString(),
            MatchingConstants.OffsetCachingTimeSpan);
    }

    private async Task ResetOffsetAsync(ICachingKey cachingKey)
    {
        await CommitOffsetAsync(cachingKey, MatchingConstants.DefaultOffset);
    }
}
