using System;
using DataStoreFramework.Orchestration;
using JetBrains.Annotations;

namespace DataStoreFramework.AzureBlob.Orchestration
{
    /// <summary>A collection of extensions to configure the Azure Blob Provider in the Data Store Orchestrator.</summary>
    [PublicAPI]
    public static class OrchestratorConfigurationProviderBuilderExtensions
    {
        /// <summary>Adds an Azure Blob Data Store Provider to the dependency injection system.</summary>
        /// <typeparam name="TOrchestratorConfigurationProvider">The orchestrator configuration provider type the builder will create.</typeparam>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to create an <see cref="AzureBlobProviderOptions"/> instance.</param>
        /// <returns>The passed in <paramref name="services"/> with the Azure Blob Data Store Provider registered.</returns>
        [NotNull]
        public static IOrchestratorConfigurationProviderBuilder<TOrchestratorConfigurationProvider> AddAzureBlobDataStore<TOrchestratorConfigurationProvider>(
            [NotNull] this IOrchestratorConfigurationProviderBuilder<TOrchestratorConfigurationProvider> services,
            [CanBeNull] Action<AzureBlobProviderOptionsBuilder> builder)
            where TOrchestratorConfigurationProvider : IOrchestratorConfigurationProvider
            => services.AddDataStore<TOrchestratorConfigurationProvider, AzureBlobProvider, AzureBlobProviderOptions, AzureBlobProviderOptionsBuilder>(builder);
    }
}
