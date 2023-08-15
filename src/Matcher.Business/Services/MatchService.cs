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
        var empty = await _cacheService.ListEmptyAsync(id);

        if (empty)
        {
            var profiles = await _profileService.GetAsync();

            await _cacheService.ListCreateAsync<Profile>(id, profiles, TimeSpan.FromMinutes(10));
        }

        return await _cacheService.ListPopAsync<Profile>(id);
    }
}
