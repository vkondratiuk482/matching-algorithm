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

    public async Task<IEnumerable<Profile>> GetAsync(MatchingMask mask, int take, int skip)
    {
        var query = _applicationContext.Profiles.AsQueryable();

        if (!string.IsNullOrEmpty(mask.Name))
        {
            query = query.Where(x => x.Name == mask.Name);
        }

        if (mask.Age != null)
        {
            query = query.Where(x => x.Age == mask.Age);
        }

        if (mask.Gender != null)
        {
            query = query.Where(x => x.Gender == mask.Gender);
        }

        query = query.Take(take);
        query = query.Skip(skip);

        var profileEntities = await query.ToListAsync();

        return ProfileAdapter.FromEntityList(profileEntities);
    }
}
