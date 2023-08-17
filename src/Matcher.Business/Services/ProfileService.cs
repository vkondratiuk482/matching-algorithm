using Matcher.Business.Core;
using Matcher.Business.Interfaces;

namespace Matcher.Business.Services;

public class ProfileService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<IEnumerable<Profile>> GetAsync(MatchingMask mask, int take = 100, int skip = 0)
    {
        return await _profileRepository.GetAsync(mask, take, skip);
    }
}
