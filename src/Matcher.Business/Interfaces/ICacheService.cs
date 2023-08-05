namespace Matcher.Business.Interfaces;

public interface ICacheService
{
    Task<bool> KeyExists(string key);
    
    Task ListPush<T>(string key, IEnumerable<T> list, int ttl);

    Task<T> ListPop<T>(string key);
}
