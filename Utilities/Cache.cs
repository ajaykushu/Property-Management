using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
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
        public void AddItem(string key, object value,long ticks)
        {
            DateTime cacheEntry;
            if (!_memoryCache.TryGetValue(key, out var val))
            {
                
                cacheEntry = DateTime.UtcNow;
                var cacheentryop = new MemoryCacheEntryOptions();
                cacheentryop.SetAbsoluteExpiration(new TimeSpan(ticks));
                //key not in cache
                _memoryCache.Set(key, value, cacheentryop);
            }

        }
        public string GetItem(string key)
        {
            var res = _memoryCache.Get(key);
            if (res==null)
            {
                return "";
            }
            else { return res as string; }
        }
       
    }
}
