using Matcher.Business.Core;
using Matcher.Business.Interfaces;

namespace Matcher.Business.Services;

public class ProfileService
{
    private static readonly string OffsetPrefix = "offset:";
    private static readonly TimeSpan OffsetTimeSpan = TimeSpan.FromMinutes(10);
    
    private readonly ICacheService _cacheService;
    private readonly IProfileRepository _profileRepository;

    public ProfileService(IProfileRepository profileRepository, ICacheService cacheService)
    {
        _cacheService = cacheService;
        _profileRepository = profileRepository;
    }

    public async Task<IEnumerable<Profile>> GetAsync(MatchingMask mask, int take = 100, int skip = 0)
    {
        return await _profileRepository.GetAsync(mask, take, skip);
    }

    public async Task<int> GetOffsetAsync(string id)
    {
        var value = await _cacheService.GetStringByKeyAsync(OffsetPrefix + id);

        return int.Parse(value);
    }

    public async Task CommitOffset(string id, int offset)
    {
        await _cacheService.SetStringByKeyAsync(OffsetPrefix + id, offset.ToString(), OffsetTimeSpan);
    }
}
