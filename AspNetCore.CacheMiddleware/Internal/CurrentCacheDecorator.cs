using Microsoft.Extensions.Options;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace AspNetCore.CacheMiddleware.Internal
{
    public class CurrentCacheDecorator: ICurrentCacheDecorator
    {
        private readonly ICurrentCache currentCache;
        private readonly StartOptions startOptions;

        public CurrentCacheDecorator(ICurrentCache currentCache, IOptions<StartOptions> startOptions)
        {
            this.currentCache = currentCache;
            this.startOptions = startOptions.Value;
        }

        public bool Contains(string key)
        {
            key = SetKey(key);
            return currentCache.Contains(key);
        }


        public T Get<T>(string key)
        {
            key = SetKey(key);
            var serialized = currentCache.Get<string>(key);
            var byteAfter64 = Convert.FromBase64String(serialized);
            using (var memoryStream = new MemoryStream(byteAfter64))
                return Serializer.Deserialize<T>(memoryStream);
            
        }

        public async void Set<T>(string key, T data, int cacheTime = 60)
        {
            key = SetKey(key);
            await AddKey(key).ConfigureAwait(false);
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, data);
                var byteArray = stream.ToArray();
                var serialized = Convert.ToBase64String(byteArray, 0, byteArray.Length);
                currentCache.Set(key, serialized, cacheTime);
            }
        }

        private Task AddKey(string key)
        {
            var allkey = $"{startOptions.NamespaceName}:{startOptions.CacheAllKey}";
            var keys = currentCache.Get<List<string>>(allkey);
            if (keys==null)
                keys = new List<string>();
            if (!keys.Contains(key))
                keys.Add(key);

            currentCache.Set(allkey, keys, 6000);

            return Task.CompletedTask;
        }

        public void Clear()
        {
            var allkey = $"{startOptions.NamespaceName}:{startOptions.CacheAllKey}";
            var keys = currentCache.Get<List<string>>(allkey);
            foreach (var key in keys)
            {
                if (keys.Contains(key))
                    currentCache.Remove(key);
            }
            keys = new List<string>();
            currentCache.Set(allkey, keys, 6000);

        }

        private string SetKey(string key)
        {
            return $"{startOptions.NamespaceName}:{key}";

        }



    }
}
