using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.Providers
{
    /// <summary>A collection of extension methods for registering Data Store Providers with the Microsoft.Extensions.DependencyInjection framework.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds a data store implementation to the dependency injection system.</summary>
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
            where TDataStoreOptionsBuilder : DataStoreProviderOptionsBuilder<TDataStoreOptions>, new()
            where TDataStoreOptions : DataStoreProviderOptions
        {
            var optionsBuilder = new TDataStoreOptionsBuilder();
            builder?.Invoke(optionsBuilder);

            var options = optionsBuilder.Build();

            return services
                .AddSingleton(options)
                .AddScoped<IDataStoreProvider, TDataStore>()
                ;
        }
    }
}
