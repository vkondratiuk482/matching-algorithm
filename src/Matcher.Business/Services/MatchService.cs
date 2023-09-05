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

        var prefix = MatchingConstants.ProfilesCachePrefix + profile.Id;

        var key = prefix + profileCriteria.ToString();

        var existingKey = await _cacheService.GetKeyByPatternAsync(prefix + "*");

        if (existingKey != key)
        {
            await _cacheService.DeleteKeyAsync(existingKey);
            await CommitOffsetAsync(profile.Id, offset: 0);
        }

        var empty = await _cacheService.ListEmptyAsync(key);

        if (!empty)
        {
            return await _cacheService.PopFromListAsync<Profile>(key);
        }

        var offset = await GetOffsetAsync(profile.Id);

        if (offset != MatchingConstants.DefaultOffset)
        {
            offset += MatchingConstants.DefaultTake;
        }

        await CommitOffsetAsync(profile.Id, offset);

        var profiles = await _profileService.GetAsync(profileCriteria, MatchingConstants.DefaultTake, offset);

        var enumerable = profiles.ToArray();

        if (enumerable.Any())
        {
            await _cacheService.CreateListAsync(key, enumerable, MatchingConstants.ProfilesCacheTimeSpan);

            return await _cacheService.PopFromListAsync<Profile>(key);
        }

        if (offset > 0)
        {
            await CommitOffsetAsync(profile.Id, 0);
        }

        return null;
    }

    private async Task<int> GetOffsetAsync(int profileId)
    {
        var value = await _cacheService.GetStringByKeyAsync(MatchingConstants.OffsetCachePrefix + profileId);

        if (value == null)
        {
            return MatchingConstants.DefaultOffset;
        }

        return int.Parse(value);
    }

    private async Task CommitOffsetAsync(int profileId, int offset)
    {
        await _cacheService.SetStringByKeyAsync(MatchingConstants.OffsetCachePrefix + profileId, offset.ToString(),
            MatchingConstants.OffsetCacheTimeSpan);
    }
}
