
using System;

namespace caching_proxy.core
{
    public interface ICacheManager
    {
        void Set(string key, byte[] value, TimeSpan ttl);
        byte[] Get(string key);
        bool TryGetValue(string key, out byte[] value);
        void Remove(string key);
        void Clear();
    }
}