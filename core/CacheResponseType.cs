namespace caching_proxy.core
{
    public enum CacheResponseType
    {
        /// <summary>
        /// The response was served directly from cache.
        /// </summary>
        HIT,

        /// <summary>
        /// The response was not found in cache and was fetched from the origin.
        /// </summary>
        MISS,

        /// <summary>
        /// The cache entry existed but had expired; it was refreshed from the origin.
        /// </summary>
        EXPIRED
        
    }
}