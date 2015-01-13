using System;
using System.Runtime.Caching;

namespace PhotoSharingApp.Models.Security
{
    public static class AccessTokenRepository
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public static void AddToCache(String cacheKeyName, Object cacheItem)
        {
            var policy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                SlidingExpiration = new TimeSpan(0, 5, 0)
            };

            Cache.Set(cacheKeyName, cacheItem, policy);
        }

        public static Object GetToken(String cacheKeyName)
        {
            return Cache[cacheKeyName];
        }

        public static void RemoveToken(String cacheKeyName)
        {
            if (Cache.Contains(cacheKeyName))
            {
                Cache.Remove(cacheKeyName);
            }
        }
    }
}