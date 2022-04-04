
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Linq;

namespace AspNetCore.CacheMiddleware
{

//    public class CustomMiddleware
//    {
//        private readonly RequestDelegate next;

//        public CustomMiddleware(RequestDelegate next)
//        {
//            this.next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            // Get the enpoint which is executing (asp.net core 3.0 only)
//            var executingEnpoint = context.GetEndpoint();

//// Get attributes on the executing action method and it's defining controller class
//            var attributes = executingEnpoint.Metadata.OfType<MyCustomAttribute>();

//            await next(context);

//            // Get the enpoint which was executed (asp.net core 2.2 possible after call to await next(context))
//            var executingEnpoint2 = context.GetEndpoint();

//// Get attributes on the executing action method and it's defining controller class
//            var attributes2 = executingEnpoint.Metadata.OfType<MyCustomAttribute>();
//        }
//    }
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
            if (endpointFeature == null|| endpointFeature.Endpoint==null)
            {
                await next(context);
                return;
            }

            var attribute = endpointFeature.Endpoint.Metadata.GetMetadata<CacheOpenAttribute>();

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
