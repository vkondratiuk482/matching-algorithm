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

    public async Task<Profile> GetByUserIdAsync(int userId)
    {
        var profileEntity = await _applicationContext.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);

        return ProfileAdapter.FromEntity(profileEntity);
    }

    // TODO: Refactor to Specification Pattern
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

        if (mask.MinScore != null)
        {
            query = query.Where(x => x.Score >= mask.MinScore);
        }

        if (mask.MaxScore != null)
        {
            query = query.Where(x => x.Score <= mask.MaxScore);
        }

        query = query.Take(take);
        query = query.Skip(skip);

        var profileEntities = await query.ToListAsync();

        return ProfileAdapter.FromEntityList(profileEntities);
    }
}
