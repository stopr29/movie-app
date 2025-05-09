using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MovieApp.Infrastructure.Config;

namespace MovieApp.Infrastructure.Caching;

public class AppCache : IAppCache
{
    private readonly IMemoryCache _cache;
    private readonly CacheConfig _config;

    public AppCache(IMemoryCache cache, IOptions<CacheConfig> options)
    {
        _cache = cache;
        _config = options.Value;
    }

    public T? Get<T>(string key) where T : notnull
    {
        _cache.TryGetValue(key, out T? value);
        return value;
    }

    public void Set<T>(string key, T value)
    {
        var options = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.Parse(_config.DefaultTtl)
        };

        _cache.Set(key, value, options);
    }
}