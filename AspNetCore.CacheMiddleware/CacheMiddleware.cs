using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.CacheMiddleware
{
    public class CacheMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ICurrentCacheDecorator currentCacheDecorator;

        public CacheMiddleware(RequestDelegate next, ICurrentCacheDecorator currentCacheDecorator)
        {
            this.next = next;
            this.currentCacheDecorator = currentCacheDecorator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpointFeature = context.Features.Get<IEndpointFeature>();
            if (endpointFeature == null)
            {
                await next(context);
                return;
            }

            var attribute = endpointFeature?.Endpoint.Metadata.GetMetadata<CacheOpenAttribute>();
            if (attribute == null)
            {
                await next(context);
                return;
            }

            string cacheKey = context.Request.Path + context.Request.QueryString;

            if (currentCacheDecorator.Contains(cacheKey))
            {
                var cache = currentCacheDecorator.Get<string>(cacheKey);
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(cache);
            }
            else
            {
                var originResponse = context.Response.Body;
                using (MemoryStream ms = new MemoryStream())
                {
                    context.Response.Body = ms;
                    await next(context);
                    //ms.Position = 0;
                     ms.Seek(0, SeekOrigin.Begin);
                    await ms.CopyToAsync(originResponse);
                    //ms.Position = 0;
                    ms.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(ms);
                    var cacheData = await reader.ReadToEndAsync();


                    currentCacheDecorator.Set(cacheKey, cacheData, attribute.ExpiryTime);
                    //originResponse.Position = 0;
                    originResponse.Seek(0, SeekOrigin.Begin);
                }
            }
        }
    }
}
