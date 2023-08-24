namespace Matcher.Business.Interfaces;

public interface ICacheService
{
    Task DeleteKeyAsync(string key);

    Task SetStringByKeyAsync(string key, string value, TimeSpan ttl);
    
    Task<string> GetStringByKeyAsync(string key);

    Task<string> GetKeyByPatternAsync(string pattern);

    Task<bool> ListEmptyAsync(string key);

    Task CreateListAsync<T>(string key, IEnumerable<T> list, TimeSpan ttl);

    Task<T> PopFromListAsync<T>(string key);
}
