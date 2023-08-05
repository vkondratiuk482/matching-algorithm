using Matcher.Business.Core;

namespace Matcher.Business.Interfaces;

public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetAsync();
}
