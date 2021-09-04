using System;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A collection of configuration provider builder extensions.</summary>
    [PublicAPI]
    public static class OrchestratorConfigurationProviderBuilderExtensions
    {
        /// <summary>Adds a new data store provider to be managed.</summary>
        /// <typeparam name="TOrchestratorConfigurationProvider">The orchestrator configuration provider type the builder will create.</typeparam>
        /// <param name="configurationBuilder">A Data Store Orchestrator configuration builder.</param>
        /// <param name="dataStoreType">The type of the provider.</param>
        /// <param name="options">The options to create a provider instance.</param>
        /// <remarks>This is generally only available during set-up, unless using a dynamic Configuration Provider.</remarks>
        public static void AddDataStore<TOrchestratorConfigurationProvider>(
            [NotNull]
            this IOrchestratorConfigurationProviderBuilder<TOrchestratorConfigurationProvider> configurationBuilder,
            [NotNull] Type dataStoreType,
            [NotNull] ProviderOptions options)
            where TOrchestratorConfigurationProvider : IOrchestratorConfigurationProvider
        {
            if (configurationBuilder is null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            configurationBuilder.AddDataStore(ProviderDescriptor.Describe(dataStoreType, options));
        }

        /// <summary>Adds a Data Store Provider to the Data Store Orchestrator.</summary>
        /// <typeparam name="TOrchestratorConfigurationProvider">The orchestrator configuration provider type the builder will create.</typeparam>
        /// <typeparam name="TDataStore">The type of the data store to register.</typeparam>
        /// <typeparam name="TDataStoreOptions">The type of the data store provider options.</typeparam>
        /// <typeparam name="TDataStoreOptionsBuilder">The type of the data store provider options builder.</typeparam>
        /// <param name="configurationBuilder">A Data Store Orchestrator configuration builder.</param>
        /// <param name="builder">A builder method to populate the <typeparamref name="TDataStoreOptionsBuilder"/> instance.</param>
        /// <returns>The passed in <paramref name="configurationBuilder"/> with the Data Store Provider registered.</returns>
        [NotNull]
        public static IOrchestratorConfigurationProviderBuilder<TOrchestratorConfigurationProvider> AddDataStore<TOrchestratorConfigurationProvider, TDataStore, TDataStoreOptions,
            TDataStoreOptionsBuilder>(
            [NotNull] this IOrchestratorConfigurationProviderBuilder<TOrchestratorConfigurationProvider> configurationBuilder,
            [CanBeNull] Action<TDataStoreOptionsBuilder> builder)
            where TOrchestratorConfigurationProvider : IOrchestratorConfigurationProvider
            where TDataStore : class, IDataStoreProvider
            where TDataStoreOptionsBuilder : IProviderOptionsBuilder<TDataStoreOptions>, new()
            where TDataStoreOptions : ProviderOptions
        {
            if (configurationBuilder is null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            var optionsBuilder = new TDataStoreOptionsBuilder();
            builder?.Invoke(optionsBuilder);

            var options = optionsBuilder.Build();

            configurationBuilder.AddDataStore(typeof(TDataStore), options);
            return configurationBuilder;
        }
    }
}
