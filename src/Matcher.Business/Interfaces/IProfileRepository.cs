using Matcher.Business.Core;

namespace Matcher.Business.Interfaces;

public interface IProfileRepository
{
    Task<Profile> GetByUserIdAsync(int userId);
    
    Task<IEnumerable<Profile>> GetAsync(ProfileCriteria criteria, int take, int skip);
}
