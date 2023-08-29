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
    public async Task<IEnumerable<Profile>> GetAsync(ProfileCriteria criteria, int take, int skip)
    {
        var query = _applicationContext.Profiles.AsQueryable();

        if (!string.IsNullOrEmpty(criteria.Name))
        {
            query = query.Where(x => x.Name == criteria.Name);
        }

        if (criteria.Age != null)
        {
            query = query.Where(x => x.Age == criteria.Age);
        }

        if (criteria.Gender != null)
        {
            query = query.Where(x => x.Gender == criteria.Gender);
        }

        if (criteria.MinScore != null)
        {
            query = query.Where(x => x.Score >= criteria.MinScore);
        }

        if (criteria.MaxScore != null)
        {
            query = query.Where(x => x.Score <= criteria.MaxScore);
        }

        query = query.Take(take);
        query = query.Skip(skip);

        var profileEntities = await query.ToListAsync();

        return ProfileAdapter.FromEntityList(profileEntities);
    }
}
