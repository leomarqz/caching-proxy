
using System;
using caching_proxy.extras;
using Microsoft.Extensions.Caching.Memory;

namespace caching_proxy.core;
public class CacheManager : ICacheManager
{
    private readonly MemoryCache _cache;
    private LogC _logc;
    public CacheManager()
    {
        _logc = new LogC("CacheManager.cs");
        try
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1024L * 1024L * 100L // 100 MB size limit
            });
        }catch(Exception ex)
        {
            _logc.Error("Failed to initialize MemoryCache: " + ex.Message);
            throw;
        }
    }

    public void Clear()
    {
        _cache.Clear();
        // _logc.Info("Cache cleared");
    }

    public byte[] Get(string key)
    {
        // _logc.Info($"Retrieving cache for key: {key}");
        return (byte[]) _cache.Get(key);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        // _logc.Info($"Removed cache for key: {key}");
    }

    public void Set(string key, byte[] value, TimeSpan ttl)
    {
        try
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl,
                Size = value.Length // Tamaño en bytes
            };

            _cache.Set(key, value, options);
            // _logc.Info($"Set cache for key: {key} ({value.Length / 1024.0:F2} KB, TTL={ttl})");
        }catch(Exception ex)
        {
            _logc.Error($"Failed to set cache for key: {key}. Error: {ex.Message}");
        }
    }

    public bool TryGetValue(string key, out byte[] value)
    {
        // _logc.Info($"Trying to retrieve cache for key: {key}");
        return _cache.TryGetValue(key, out value);
    }
}