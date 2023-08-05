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

    public async Task<IEnumerable<Profile>> GetAsync()
    {
        return await _profileRepository.GetAsync();
    }
}
