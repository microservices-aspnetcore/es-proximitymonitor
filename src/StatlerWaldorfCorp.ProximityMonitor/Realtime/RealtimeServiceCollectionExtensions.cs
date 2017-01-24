using System;
using Microsoft.Extensions.DependencyInjection;
using PubnubApi;

namespace StatlerWaldorfCorp.ProximityMonitor.Realtime
{
    public static class RealtimeServiceCollectionExtensions
    {
        public static IServiceCollection AddRealtimeService(this IServiceCollection services)
        {
            services.AddTransient<PubnubFactory>();
            
            return AddInternal(services, p => p.GetRequiredService<PubnubFactory>(), ServiceLifetime.Singleton);
        }

        private static IServiceCollection AddInternal(
            this IServiceCollection collection,
            Func<IServiceProvider, PubnubFactory> factoryProvider,
            ServiceLifetime lifetime) 
        {
            Func<IServiceProvider, object> factoryFunc = provider =>
            {
                var factory = factoryProvider(provider);
                return factory.CreateInstance();
            };
            
            var descriptor = new ServiceDescriptor(typeof(Pubnub), factoryFunc, lifetime);
            collection.Add(descriptor);
            return collection;
        }
    }
}