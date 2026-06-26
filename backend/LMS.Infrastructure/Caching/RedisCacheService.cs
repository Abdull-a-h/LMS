using LMS.Application.Common.Interfaces;

namespace LMS.Infrastructure.Caching;

/// <summary>
/// Redis-backed cache used for (1) book list/detail response caching with prefix invalidation
/// and (2) the refresh-token revocation set. Backed by StackExchange.Redis IConnectionMultiplexer.
/// </summary>
public class RedisCacheService : ICacheService
{
    // TODO: inject IConnectionMultiplexer; JSON-serialise values; implement prefix scan for RemoveByPrefixAsync.

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
