using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware.Internal
{
    class CurrentCache : ICurrentCache
    {
        private static MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());


        public  T Get<T>(string key)
        {
            T objValue;
            if (!string.IsNullOrEmpty(key) && Cache.TryGetValue<T>(key, out objValue))
            {
                return objValue;
            }
            else
            {
                return default(T);
            }
        }

        public  void Set<T>(string key, T data, int cacheTime = 30)
        {
            if (data == null || string.IsNullOrEmpty(key))
                return;
            
            Cache.Set<T>(key, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            });
        }

        public bool Contains(string key)
        {
            object objValue = null;
            return !string.IsNullOrEmpty(key) && Cache.TryGetValue(key, out objValue);
        }



        public void Remove(string key)
        {
            Cache.Remove(key);
        }


    }
}
