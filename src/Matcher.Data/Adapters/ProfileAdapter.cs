using Matcher.Business.Core;
using Matcher.Data.Entities;

namespace Matcher.Data.Adapters;

public static class ProfileAdapter
{
    public static IEnumerable<Profile> FromEntityList(IEnumerable<ProfileEntity> entities)
    {
        return entities.Select(entity => new Profile
        {
            Id = entity.Id,
            Age = entity.Age,
            Name = entity.Name,
            Gender = entity.Gender,
            Description = entity.Description,
        });
    }
}
