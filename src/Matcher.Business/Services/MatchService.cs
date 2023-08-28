using Matcher.Business.Core;
using Matcher.Business.Interfaces;

namespace Matcher.Business.Services;

public class MatchService
{
    private readonly ICacheService _cacheService;
    private readonly ProfileService _profileService;

    private static readonly int ScoreDelta = 1;
    
    private static readonly int DefaultTake = 100;
    private static readonly int DefaultOffset = 0;

    private static readonly string OffsetCachePrefix = "offset:";
    private static readonly TimeSpan OffsetCacheTimeSpan = TimeSpan.FromMinutes(10);

    private static readonly string ProfilesCachePrefix = "profiles:";
    private static readonly TimeSpan ProfilesCacheTimeSpan = TimeSpan.FromMinutes(10);

    public MatchService(ICacheService cacheService, ProfileService profileService)
    {
        _cacheService = cacheService;
        _profileService = profileService;
    }

    public async Task<Profile?> GetAsync(int userId, MatchingMask mask)
    {
        var profile = await _profileService.GetByUserIdAsync(userId);

        mask.MinScore = profile.Score - ScoreDelta;
        mask.MaxScore = profile.Score + ScoreDelta;

        var prefix = ProfilesCachePrefix + profile.Id;

        var key = prefix + mask.ToString();

        var existingKey = await _cacheService.GetKeyByPatternAsync(prefix + "*");

        if (existingKey != key)
        {
            await _cacheService.DeleteKeyAsync(existingKey);
        }

        var empty = await _cacheService.ListEmptyAsync(key);

        if (empty)
        {
            var offset = await GetOffsetAsync(profile.Id);

            if (offset != 0)
            {
                offset += DefaultTake;
            }

            await CommitOffsetAsync(profile.Id, offset);

            var profiles = await _profileService.GetAsync(mask, DefaultTake, offset);

            var enumerable = profiles.ToArray();

            if (!enumerable.Any())
            {
                return null;
            }

            await _cacheService.CreateListAsync<Profile>(key, enumerable, ProfilesCacheTimeSpan);
        }

        return await _cacheService.PopFromListAsync<Profile>(key);
    }

    private async Task<int> GetOffsetAsync(int profileId)
    {
        var value = await _cacheService.GetStringByKeyAsync(OffsetCachePrefix + profileId);

        if (value == null)
        {
            return 0;
        }

        return int.Parse(value);
    }

    private async Task CommitOffsetAsync(int profileId, int offset)
    {
        await _cacheService.SetStringByKeyAsync(OffsetCachePrefix + profileId, offset.ToString(), OffsetCacheTimeSpan);
    }
}
