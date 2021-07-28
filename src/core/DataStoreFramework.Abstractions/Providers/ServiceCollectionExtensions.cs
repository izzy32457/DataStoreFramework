using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DataStoreFramework.Providers
{
    /// <summary>A collection of extension methods for registering Data Store Providers with the Microsoft.Extensions.DependencyInjection framework.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds a Data Store Provider to the dependency injection system.</summary>
        /// <typeparam name="TDataStore">The type of the data store to register.</typeparam>
        /// <typeparam name="TDataStoreOptions">The type of the data store provider options.</typeparam>
        /// <typeparam name="TDataStoreOptionsBuilder">The type of the data store provider options builder.</typeparam>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to populate the <typeparamref name="TDataStoreOptionsBuilder"/> instance.</param>
        /// <returns>The passed in <paramref name="services"/> with the Data Store Provider registered.</returns>
        [NotNull]
        public static IServiceCollection AddDataStore<TDataStore, TDataStoreOptions, TDataStoreOptionsBuilder>(
            [NotNull] this IServiceCollection services,
            [CanBeNull] Action<TDataStoreOptionsBuilder> builder)
            where TDataStore : class, IDataStoreProvider
            where TDataStoreOptionsBuilder : IProviderOptionsBuilder<TDataStoreOptions>, new()
            where TDataStoreOptions : ProviderOptions
            => services.AddDataStore<TDataStore, TDataStoreOptions, TDataStoreOptionsBuilder>(builder, ServiceLifetime.Scoped);

        /// <summary>Adds a Data Store Provider to the dependency injection system with the specified lifetime.</summary>
        /// <typeparam name="TDataStore">The type of the data store to register.</typeparam>
        /// <typeparam name="TDataStoreOptions">The type of the data store provider options.</typeparam>
        /// <typeparam name="TDataStoreOptionsBuilder">The type of the data store provider options builder.</typeparam>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to populate the <typeparamref name="TDataStoreOptionsBuilder"/> instance.</param>
        /// <param name="serviceLifetime">The required service lifetime for the Data Store Provider.</param>
        /// <returns>The passed in <paramref name="services"/> with the Data Store Provider registered.</returns>
        [NotNull]
        public static IServiceCollection AddDataStore<TDataStore, TDataStoreOptions, TDataStoreOptionsBuilder>(
            [NotNull] this IServiceCollection services,
            [CanBeNull] Action<TDataStoreOptionsBuilder> builder,
            ServiceLifetime serviceLifetime)
            where TDataStore : class, IDataStoreProvider
            where TDataStoreOptionsBuilder : IProviderOptionsBuilder<TDataStoreOptions>, new()
            where TDataStoreOptions : ProviderOptions
        {
            var optionsBuilder = new TDataStoreOptionsBuilder();
            builder?.Invoke(optionsBuilder);

            var options = optionsBuilder.Build();

            services.TryAddSingleton(options);
            services.TryAdd(ServiceDescriptor.Singleton<ProviderOptions>(sp => sp.GetRequiredService<TDataStoreOptions>()));
            services.TryAdd(ServiceDescriptor.Describe(typeof(TDataStore), typeof(TDataStore), serviceLifetime));
            services.TryAdd(ServiceDescriptor.Describe(typeof(IDataStoreProvider), sp => sp.GetRequiredService<TDataStore>(), serviceLifetime));

            return services;
        }
    }
}
