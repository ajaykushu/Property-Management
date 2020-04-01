using Microsoft.Extensions.Caching.Memory;
using System;
using Utilities.Interface;

namespace Utilities
{
    public class Cache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public Cache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void AddItem(string key, object value, long ticks)
        {
            if (!_memoryCache.TryGetValue(key, out _))
            {
                var cacheentryop = new MemoryCacheEntryOptions();
                cacheentryop.SetAbsoluteExpiration(new TimeSpan(ticks));
                _memoryCache.Set(key, value, cacheentryop);
            }
        }

        public object GetItem(string key)
        {
            var res = _memoryCache.Get(key);
            return res;
        }

        public void RemoveItem(string v)
        {
            if (_memoryCache.TryGetValue(v, out _))
                _memoryCache.Remove(v);
        }
    }
}