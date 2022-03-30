using AspNetCore.CacheMiddleware.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware
{
    public static class AspNetCoreCacheCollectionExtensions
    {

        public static IServiceCollection AddAspCoreCache(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddSingleton<ICurrentCache, CurrentCache>();
            services.AddSingleton<ICurrentCacheDecorator, CurrentCacheDecorator>();
            services.AddSingleton<IResetCache, ResetCache>();
            return services;
        }

        public static IServiceCollection AddAspCoreCache(this IServiceCollection services, Action<StartOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddSingleton<ICurrentCache, CurrentCache>();
            services.AddSingleton<ICurrentCacheDecorator, CurrentCacheDecorator>();
            services.AddSingleton<IResetCache, ResetCache>();
            services.Configure(setupAction);
            return services;
        }

        public static IServiceCollection AddAspCoreCache<CurrentCache>(this IServiceCollection services)where CurrentCache: ICurrentCache
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            var serviceType = typeof(ICurrentCache);
            var implementationInstance = typeof(CurrentCache);
            services.AddSingleton(serviceType, implementationInstance);
            services.AddSingleton<ICurrentCacheDecorator, CurrentCacheDecorator>();
            services.AddSingleton<IResetCache, ResetCache>();
            return services;
        }

        public static IServiceCollection AddAspCoreCache<CurrentCache>(this IServiceCollection services, Action<StartOptions> setupAction) where CurrentCache : ICurrentCache
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            var serviceType = typeof(ICurrentCache);
            var implementationInstance = typeof(CurrentCache);
            services.AddSingleton(serviceType, implementationInstance);
            services.AddSingleton<ICurrentCacheDecorator, CurrentCacheDecorator>();
            services.AddSingleton<IResetCache, ResetCache>();
            services.Configure(setupAction);
            return services;
        }
    }
}
