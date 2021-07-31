using System;
using System.Collections.Generic;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A Data Store Configuration provider for creating Orchestrator instance.</summary>
    [PublicAPI]
    public class OrchestratorStaticConfigurationProviderBuilder : IOrchestratorConfigurationProviderBuilder
    {
        private readonly List<ProviderDescriptor> _registeredProviders;

        /// <summary>Initializes a new instance of the <see cref="OrchestratorStaticConfigurationProviderBuilder"/> class.</summary>
        public OrchestratorStaticConfigurationProviderBuilder()
        {
            _registeredProviders = new ();
        }

        /// <summary>Adds a new data store provider to be managed.</summary>
        /// <param name="dataStoreType">The type of the provider.</param>
        /// <param name="options">The options to create a provider instance.</param>
        public void AddDataStore(Type dataStoreType, ProviderOptions options)
            => _registeredProviders.Add(ProviderDescriptor.Describe(dataStoreType, options));

        /// <summary>Creates an instance of <see cref="OrchestratorStaticConfigurationProvider"/>.</summary>
        /// <returns>A new instance of <see cref="OrchestratorStaticConfigurationProvider"/> containing the registered providers.</returns>
        [NotNull]
        public OrchestratorStaticConfigurationProvider Build()
            => new (_registeredProviders);
    }
}
