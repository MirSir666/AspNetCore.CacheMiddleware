using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware
{
    public static class AspNetCoreCacheBuilderExtensions
    {
        public static IApplicationBuilder UseAspCoreCache(this IApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            builder.UseCacheMiddleware();
            
            return builder;
        }
    }
}
