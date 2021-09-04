using System.Collections.Generic;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A Data Store Configuration provider for creating Orchestrator instance.</summary>
    [PublicAPI]
    public class OrchestratorStaticConfigurationProviderBuilder : IOrchestratorConfigurationProviderBuilder<OrchestratorStaticConfigurationProvider>
    {
        private readonly List<ProviderDescriptor> _registeredProviders;

        /// <summary>Initializes a new instance of the <see cref="OrchestratorStaticConfigurationProviderBuilder"/> class.</summary>
        public OrchestratorStaticConfigurationProviderBuilder()
        {
            _registeredProviders = new ();
        }

        /// <inheritdoc cref="IOrchestratorConfigurationProviderBuilder{OrchestratorStaticConfigurationProvider}.AddDataStore(ProviderDescriptor)"/>
        public void AddDataStore(ProviderDescriptor providerDescriptor)
            => _registeredProviders.Add(providerDescriptor);

        /// <inheritdoc cref="IOrchestratorConfigurationProviderBuilder{OrchestratorStaticConfigurationProvider}.AddDataStoreRange(IEnumerable{ProviderDescriptor})"/>
        public void AddDataStoreRange(IEnumerable<ProviderDescriptor> providerDescriptors)
            => _registeredProviders.AddRange(providerDescriptors);

        /// <inheritdoc cref="IOrchestratorConfigurationProviderBuilder{OrchestratorStaticConfigurationProvider}.Build()"/>
        public OrchestratorStaticConfigurationProvider Build()
            => new (_registeredProviders);
    }
}
