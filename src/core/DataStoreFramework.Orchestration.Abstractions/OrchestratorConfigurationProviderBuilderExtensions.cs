using System;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>Acollection of configuration provider builder extensions.</summary>
    [PublicAPI]
    public static class OrchestratorConfigurationProviderBuilderExtensions
    {
        /// <summary>Adds a Data Store Provider to the Data Store Orchestrator.</summary>
        /// <typeparam name="TDataStore">The type of the data store to register.</typeparam>
        /// <typeparam name="TDataStoreOptions">The type of the data store provider options.</typeparam>
        /// <typeparam name="TDataStoreOptionsBuilder">The type of the data store provider options builder.</typeparam>
        /// <param name="configurationBuilder">A Data Store Orchestrator configuration builder.</param>
        /// <param name="builder">A builder method to populate the <typeparamref name="TDataStoreOptionsBuilder"/> instance.</param>
        /// <returns>The passed in <paramref name="configurationBuilder"/> with the Data Store Provider registered.</returns>
        [NotNull]
        public static IOrchestratorConfigurationProviderBuilder AddDataStore<TDataStore, TDataStoreOptions,
            TDataStoreOptionsBuilder>(
            [NotNull] this IOrchestratorConfigurationProviderBuilder configurationBuilder,
            [CanBeNull] Action<TDataStoreOptionsBuilder> builder)
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
