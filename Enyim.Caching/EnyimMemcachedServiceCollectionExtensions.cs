﻿using Enyim.Caching;
using Enyim.Caching.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EnyimMemcachedServiceCollectionExtensions
    {
        public static IServiceCollection AddEnyimMemcached(this IServiceCollection services, Action<MemcachedClientOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            return AddEnyimMemcached(services, s => s.Configure(setupAction));
        }

        public static IServiceCollection AddEnyimMemcached(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return AddEnyimMemcached(services, s => s.Configure<MemcachedClientOptions>(configuration));
        }

        private static IServiceCollection AddEnyimMemcached(IServiceCollection services, Action<IServiceCollection> configure)
        {
            services.AddOptions();
            configure(services);
            services.AddTransient<IMemcachedClientConfiguration, MemcachedClientConfiguration>();
            services.AddSingleton<MemcachedClient, MemcachedClient>();

            services.AddSingleton<IMemcachedClient>(factory => factory.GetService<MemcachedClient>());
            services.AddSingleton<IDistributedCache>(factory => factory.GetService<MemcachedClient>());

            return services;
        }
    }
}
