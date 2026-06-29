using System.Text.Json;
using LMS.Application.Common.Interfaces;
using StackExchange.Redis;

namespace LMS.Infrastructure.Caching;

/// <summary>
/// Redis-backed cache used for (1) book list/detail response caching with prefix invalidation
/// and (2) the refresh-token revocation set. Values are stored as JSON strings.
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private IDatabase Db => _redis.GetDatabase();

    public RedisCacheService(IConnectionMultiplexer redis) => _redis = redis;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await Db.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value.ToString());
    }

    public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
        => Db.StringSetAsync(key, JsonSerializer.Serialize(value), ttl);

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => Db.KeyDeleteAsync(key);

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        // Scan every key matching "prefix*" across each server and delete them.
        foreach (var endpoint in _redis.GetEndPoints())
        {
            var server = _redis.GetServer(endpoint);
            await foreach (var key in server.KeysAsync(pattern: $"{prefix}*").WithCancellation(cancellationToken))
            {
                await Db.KeyDeleteAsync(key);
            }
        }
    }
}
