using System;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.Orchestration
{
    /// <summary>Defines methods needed to set-up an Orchestrator Configuration provider.</summary>
    [PublicAPI]
    public interface IOrchestratorConfigurationProviderBuilder
    {
        /// <summary>Adds a new data store provider to be managed.</summary>
        /// <param name="dataStoreType">The type of the provider.</param>
        /// <param name="options">The options to create a provider instance.</param>
        /// <remarks>This is generally only available during set-up, unless using a dynamic Configuration Provider.</remarks>
        void AddDataStore([NotNull] Type dataStoreType, [NotNull] ProviderOptions options);
    }
}
