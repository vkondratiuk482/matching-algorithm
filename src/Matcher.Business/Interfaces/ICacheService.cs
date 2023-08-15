namespace Matcher.Business.Interfaces;

public interface ICacheService
{
    Task<bool> ListEmptyAsync(string key);
    
    Task ListCreateAsync<T>(string key, IEnumerable<T> list, TimeSpan ttl);

    Task<T> ListPopAsync<T>(string key);
}
