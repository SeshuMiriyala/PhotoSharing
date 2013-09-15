using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace PhotoSharingApp.Models.Security
{
    public static class AccessTokenRepository
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        //private static readonly ObjectCache RemovedCache = MemoryCache.Default;
        //private static CacheEntryRemovedCallback _callback;

        public static void AddToCache(String cacheKeyName, Object cacheItem)
        {
            var policy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                SlidingExpiration = new TimeSpan(0, 5, 0)
                //RemovedCallback = MyCachedItemRemovedCallback
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

        //private static void MyCachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        //{
        //    RemovedCache.Set(arguments.CacheItem,
        //                     new CacheItemPolicy
        //                         {
        //                             AbsoluteExpiration = DateTime.Now.AddMinutes(10.00),
        //                             Priority = CacheItemPriority.Default
        //                         });
        //    var strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), " | Key - Name:", arguments.CacheItem.Key, " | Value - Object:", arguments.CacheItem.Value.ToString());
        //}
    }
}