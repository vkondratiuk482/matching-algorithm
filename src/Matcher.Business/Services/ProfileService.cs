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

    public async Task<Profile> GetByUserIdAsync(int id)
    {
        return await _profileRepository.GetByUserIdAsync(id);
    }

    public async Task<IEnumerable<Profile>> GetAsync(ProfileCriteria criteria, int take = 100, int skip = 0)
    {
        return await _profileRepository.GetAsync(criteria, take, skip);
    }
}
