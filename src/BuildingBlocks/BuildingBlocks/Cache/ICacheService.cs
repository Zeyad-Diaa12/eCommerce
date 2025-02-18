using Microsoft.Extensions.Caching.Distributed;

namespace BuildingBlocks.Cache;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options = null);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
}
