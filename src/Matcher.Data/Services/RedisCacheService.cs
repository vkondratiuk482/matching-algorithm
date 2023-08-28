using System.Text.Json;
using StackExchange.Redis;
using Matcher.Business.Interfaces;

namespace Matcher.Data.Services;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string?> GetStringByKeyAsync(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();

        var result = await database.StringGetAsync(key);

        if (!result.HasValue)
        {
            return null;
        }

        return result.ToString();
    }

    public async Task SetStringByKeyAsync(string key, string value, TimeSpan ttl)
    {
        var database = _connectionMultiplexer.GetDatabase();

        await database.StringSetAsync(key, value, ttl);
    }

    public async Task DeleteKeyAsync(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();

        await database.KeyDeleteAsync(key);
    }

    public async Task<string> GetKeyByPatternAsync(string pattern)
    {
        var keys = new List<RedisKey>();

        foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endpoint);

            keys.AddRange(server.Keys(pattern: pattern));
        }

        return keys.FirstOrDefault().ToString();
    }

    public async Task<bool> ListEmptyAsync(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();

        var length = await database.ListLengthAsync(key);

        return length == 0;
    }

    public async Task CreateListAsync<T>(string key, IEnumerable<T> list, TimeSpan ttl)
    {
        var transaction = _connectionMultiplexer.GetDatabase().CreateTransaction();

        foreach (var item in list)
        {
            var serialized = JsonSerializer.Serialize(item);

            transaction.ListRightPushAsync(key, new RedisValue(serialized));
        }

        transaction.KeyExpireAsync(key, ttl);

        await transaction.ExecuteAsync();
    }

    public async Task<T> PopFromListAsync<T>(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();

        var value = await database.ListRightPopAsync(key);

        if (!value.HasValue)
        {
            throw new InvalidOperationException();
        }

        return JsonSerializer.Deserialize<T>(value.ToString());
    }
}
