using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataStoreFramework.Orchestration.Exceptions;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A static Orchestrator Configuration provider.</summary>
    [PublicAPI]
    public class OrchestratorStaticConfigurationProvider : IOrchestratorConfigurationProvider
    {
        private readonly List<ProviderDescriptor> _providers;

        private readonly ConcurrentDictionary<string, IDataStoreProvider> _providerInstances;

        /// <summary>Initializes a new instance of the <see cref="OrchestratorStaticConfigurationProvider"/> class.</summary>
        /// <param name="providers">A set of registered providers.</param>
        internal OrchestratorStaticConfigurationProvider([NotNull][ItemNotNull] IEnumerable<ProviderDescriptor> providers)
        {
            _providers = providers.ToList();
            _providerInstances = new ();
        }

        /// <inheritdoc/>
        public IDataStoreProvider GetDataStoreByName(string name, IServiceProvider services)
            => _providerInstances.GetOrAdd(name, n =>
                {
                    var provider = _providers.SingleOrDefault(p => p.Identifier == n);
                    if (provider is null)
                    {
                        throw new ProviderNotFoundException($"A provider was not found with the name '{n}'");
                    }

                    return ActivatorUtilities.CreateInstance(services, provider.ProviderType, provider.Options) as IDataStoreProvider
                           ?? throw new OrchestrationException(
                               $"Unable to create an instance of {provider.ProviderType.FullName}");
                });

        /// <inheritdoc/>
        public IDataStoreProvider GetDataStoreByObjectPath(string objectPath, IServiceProvider services)
        {
            foreach (var descriptor in _providers)
            {
                try
                {
                    if (ActivatorUtilities.CreateInstance(services, descriptor.ProviderType, descriptor.Options) is not IDataStoreProvider instance ||
                        !instance.CanAccessObject(objectPath))
                    {
                        continue;
                    }

                    return _providerInstances.GetOrAdd(descriptor.Identifier, _ => instance);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    // do nothing
                }
            }

            throw new ProviderNotFoundException($"No provider was found for the object path '{objectPath}'.");
        }
    }
}
