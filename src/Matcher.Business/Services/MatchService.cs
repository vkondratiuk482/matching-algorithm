using System.Text;
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

    public async Task<Profile?> GetAsync(string id, MatchingMask mask)
    {
        var prefix = $"profiles:{id}:";

        var key = prefix + mask.ToString();
        
        var existingKey = await _cacheService.KeyGetByPatternAsync(prefix + "*");

        if (existingKey != key)
        {
            await _cacheService.KeyDeleteAsync(existingKey);
        }

        var empty = await _cacheService.ListEmptyAsync(key);

        if (empty)
        {
            var profiles = await _profileService.GetAsync(mask);

            var enumerable = profiles.ToArray();
            
            if (!enumerable.Any())
            {
                return null;
            }
            
            await _cacheService.ListCreateAsync<Profile>(key, enumerable, TimeSpan.FromMinutes(10));
        }

        return await _cacheService.ListPopAsync<Profile>(key);
    }
}
