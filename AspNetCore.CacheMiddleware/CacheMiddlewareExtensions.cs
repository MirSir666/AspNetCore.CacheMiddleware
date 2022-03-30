using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware
{
    public static class CacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheMiddleware(
         this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheMiddleware>();
        }
    }
}
