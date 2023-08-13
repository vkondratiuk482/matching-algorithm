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
        /**
         * Think of a proper way to remove "magic".
         * For example, if we use .ListPopAsync() on the last value in the list, KeyExistsAsync returns false.
         * This behavior is desired, but still too implicit.
         */
        var cached = await _cacheService.KeyExistsAsync(id);

        if (!cached)
        {
            var profiles = await _profileService.GetAsync();

            await _cacheService.ListCreateAsync<Profile>(id, profiles, TimeSpan.FromMinutes(10));
        }
        
        return await _cacheService.ListPopAsync<Profile>(id);
    }
}
