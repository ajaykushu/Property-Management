using Microsoft.Extensions.Caching.Memory;
using System;
using Utilities.Interface;

namespace Utilities
{
    public class Cache : ICache
    {
        private IMemoryCache _memoryCache;
        public Cache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void AddItem(string key, object value)
        {
            DateTime cacheEntry;
            if (!_memoryCache.TryGetValue(key, out var val))
            {
                cacheEntry = DateTime.UtcNow;
                var cacheentryop = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));
                //key not in cache
                _memoryCache.Set(key, value, cacheentryop);
            }

        }
        public object GetItem(string key)
        {
            if (!_memoryCache.TryGetValue(key, out var val))
            {
                return null;
            }
            else { return val; }
        }
    }
}
