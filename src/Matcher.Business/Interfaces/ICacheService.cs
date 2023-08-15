namespace Matcher.Business.Interfaces;

public interface ICacheService
{
    Task<bool> ExistsAsync(string key);
    
    Task ListCreateAsync<T>(string key, IEnumerable<T> list, TimeSpan ttl);

    Task<T> ListPopAsync<T>(string key);
}
