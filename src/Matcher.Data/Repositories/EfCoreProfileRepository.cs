using Matcher.Business.Core;
using Matcher.Data.Adapters;
using Matcher.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Matcher.Data.Repositories;

public class EfCoreProfileRepository : IProfileRepository
{
    private readonly ApplicationContext _applicationContext;

    public EfCoreProfileRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IEnumerable<Profile>> GetAsync()
    {
        var profileEntities = await _applicationContext.Profiles.ToListAsync();

        return ProfileAdapter.FromEntityList(profileEntities);
    }
}
