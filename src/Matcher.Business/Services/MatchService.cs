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

    public async Task<Profile> GetAsync(string id)
    {
        var cached = await _cacheService.KeyExists(id);

        if (!cached)
        {
            var profiles = await _profileService.GetAsync();

            await _cacheService.ListPush(id, profiles, 100000);
        }

        return await _cacheService.ListPop<Profile>(id);
    }
}
