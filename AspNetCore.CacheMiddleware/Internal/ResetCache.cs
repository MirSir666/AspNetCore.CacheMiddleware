using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware.Internal
{
    public class ResetCache: IResetCache
    {
        private readonly ICurrentCacheDecorator currentCacheDecorator;

        public ResetCache(ICurrentCacheDecorator currentCacheDecorator)
        {
            this.currentCacheDecorator = currentCacheDecorator;
        }

        public void Reset()
        {
            currentCacheDecorator.Clear();
        }
    }
}
