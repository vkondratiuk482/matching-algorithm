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

    public async Task<Profile> GetAsync(string id, MatchingMask mask)
    {
        var key = FormCachingKey(id, mask);
        
        var empty = await _cacheService.ListEmptyAsync(key);

        if (empty)
        {
            var profiles = await _profileService.GetAsync(mask);

            await _cacheService.ListCreateAsync<Profile>(key, profiles, TimeSpan.FromMinutes(10));
        }

        return await _cacheService.ListPopAsync<Profile>(key);
    }

    private string FormCachingKey(string id, MatchingMask mask)
    {
        var pieces = new List<string>();

        pieces.Add($"name: {mask.Name}");
        pieces.Add($"age: {mask.Age}");
        pieces.Add($"gender: {mask.Gender}");

        return string.Join(",", pieces);
    }
}
