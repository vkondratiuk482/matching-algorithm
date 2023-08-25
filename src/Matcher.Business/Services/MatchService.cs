using System.Text;
using Matcher.Business.Core;
using Matcher.Business.Interfaces;

namespace Matcher.Business.Services;

public class MatchService
{
    private readonly ICacheService _cacheService;
    private readonly ProfileService _profileService;

    private static readonly int DefaultTake = 100;
    private static readonly int DefaultOffset = 0;

    public MatchService(ICacheService cacheService, ProfileService profileService)
    {
        _cacheService = cacheService;
        _profileService = profileService;
    }

    public async Task<Profile?> GetAsync(string id, MatchingMask mask)
    {
        var prefix = $"profiles:{id}:";

        var key = prefix + mask.ToString();
        
        var existingKey = await _cacheService.GetKeyByPatternAsync(prefix + "*");

        if (existingKey != key)
        {
            await _cacheService.DeleteKeyAsync(existingKey);
        }

        var empty = await _cacheService.ListEmptyAsync(key);

        if (empty)
        {
            var offset = await _profileService.GetOffsetAsync(id);

            if (offset != 0)
            {
                offset += DefaultTake;
            }

            await _profileService.CommitOffset(id, offset);

            // Add sorting in by createdAt/updatedAt by DESCENDING order
            // This way we can display the latest profiles
            var profiles = await _profileService.GetAsync(mask, DefaultTake, offset);

            var enumerable = profiles.ToArray();
            
            if (!enumerable.Any())
            {
                return null;
            }
            
            await _cacheService.CreateListAsync<Profile>(key, enumerable, TimeSpan.FromMinutes(10));
        }

        return await _cacheService.PopFromListAsync<Profile>(key);
    }
}
