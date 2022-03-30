using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware
{
    public interface ICurrentCacheDecorator
    {
        T Get<T>(string key);
        void Set<T>(string key, T data, int cacheTime = 60);
        bool Contains(string key);
        void Clear();
    }
}
