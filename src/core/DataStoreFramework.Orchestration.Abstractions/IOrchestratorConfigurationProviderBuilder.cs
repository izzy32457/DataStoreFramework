using System.Collections.Generic;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>Defines methods needed to set-up an Orchestrator Configuration provider.</summary>
    /// <typeparam name="TOrchestratorConfigurationProvider">The orchestrator configuration provider type the builder will create.</typeparam>
    [PublicAPI]
    public interface IOrchestratorConfigurationProviderBuilder<out TOrchestratorConfigurationProvider>
        where TOrchestratorConfigurationProvider : IOrchestratorConfigurationProvider
    {
        /// <summary>Adds a new data store provider to be managed.</summary>
        /// <param name="providerDescriptor">An instance of a provider descriptor.</param>
        /// <remarks>This is generally only available during set-up, unless using a dynamic Configuration Provider.</remarks>
        void AddDataStore([NotNull] ProviderDescriptor providerDescriptor);

        /// <summary>Adds a new data store provider to be managed.</summary>
        /// <param name="providerDescriptors">A collection of provider descriptors.</param>
        /// <remarks>This is generally only available during set-up, unless using a dynamic Configuration Provider.</remarks>
        void AddDataStoreRange([NotNull] IEnumerable<ProviderDescriptor> providerDescriptors);

        /// <summary>Creates an instance of <typeparamref name="TOrchestratorConfigurationProvider"/>.</summary>
        /// <returns>A new instance of <typeparamref name="TOrchestratorConfigurationProvider"/> containing the registered providers.</returns>
        [NotNull]
        TOrchestratorConfigurationProvider Build();
    }
}
