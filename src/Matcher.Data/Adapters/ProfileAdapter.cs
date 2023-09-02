using Matcher.Business.Core;
using Matcher.Data.Entities;

namespace Matcher.Data.Adapters;

public static class ProfileAdapter
{
    public static Profile FromEntity(ProfileEntity entity)
    {
        return new Profile
        {
            Id = entity.Id,
            Age = entity.Age,
            Name = entity.Name,
            Score = entity.Score,
            Gender = entity.Gender,
            UserId = entity.UserId,
            Description = entity.Description,
            UncommittedScore = entity.UncommittedScore,
        };
    }

    public static IEnumerable<Profile> FromEntityList(IEnumerable<ProfileEntity> entities)
    {
        return entities.Select(entity => FromEntity(entity));
    }
}
