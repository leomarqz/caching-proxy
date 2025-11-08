
using System;

namespace caching_proxy.core
{
    public class CacheProxyConfiguration
    {
        public int Port { get; set; }
        public string Origin { get; set; }
        public TimeSpan TTL { get; set; }
    }
}