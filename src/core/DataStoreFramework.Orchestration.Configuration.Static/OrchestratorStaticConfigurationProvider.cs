using System;
using System.Collections.Generic;
using System.Linq;
using DataStoreFramework.Orchestration.Exceptions;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A static Orchestrator Configuration provider.</summary>
    [PublicAPI]
    public class OrchestratorStaticConfigurationProvider : IOrchestratorConfigurationProvider
    {
        private readonly List<ProviderDescriptor> _providers;

        /// <summary>Initializes a new instance of the <see cref="OrchestratorStaticConfigurationProvider"/> class.</summary>
        /// <param name="providers">A set of registered providers.</param>
        internal OrchestratorStaticConfigurationProvider([NotNull][ItemNotNull] IEnumerable<ProviderDescriptor> providers)
        {
            _providers = providers.ToList();
        }

        /// <inheritdoc/>
        public IDataStoreProvider GetDataStoreByName(string name)
        {
            var provider = _providers.SingleOrDefault(p => p.Identifier == name);
            if (provider is null)
            {
                throw new ProviderNotFoundException($"A provider was not found with the name '{name}'");
            }

            // TODO: cache these created instances
            return Activator.CreateInstance(provider.ProviderType, provider.Options) as IDataStoreProvider
                   ?? throw new OrchestrationException($"Unable to create an instance of {provider.ProviderType.FullName}");
        }

        /// <inheritdoc/>
        public IDataStoreProvider GetDataStoreByObjectPath(string objectPath)
        {
            foreach (var descriptor in _providers)
            {
                // TODO: detect if each provider can be generated from the objectPath
            }

            throw new NotImplementedException();
        }
    }
}
