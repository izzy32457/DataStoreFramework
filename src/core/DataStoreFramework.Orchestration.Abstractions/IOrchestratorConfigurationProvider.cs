using System;
using DataStoreFramework.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.Orchestration
{
    public interface IOrchestratorConfigurationProvider
    {
        // TODO: provide crud for registering Data Store Providers

        /// <summary>Adds a new data store provider to be managed.</summary>
        /// <param name="dataStoreType">The type of the provider.</param>
        /// <param name="options">The options to create a provider instance.</param>
        /// <param name="lifetime">The lifetime of the provider.</param>
        void AddDataStore(Type dataStoreType, ProviderOptions options, ServiceLifetime lifetime = ServiceLifetime.Singleton);
    }
}
