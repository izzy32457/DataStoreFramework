using System;
using System.Collections.Generic;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.Orchestration
{
    [PublicAPI]
    public class OrchestratorStaticConfigurationProvider : IOrchestratorConfigurationProvider
    {
        public readonly List<ProviderDescriptor> RegisteredProviders;

        /// <summary>Initializes a new instance of the <see cref="OrchestratorStaticConfigurationProvider"/> class.</summary>
        public OrchestratorStaticConfigurationProvider()
        {
            RegisteredProviders = new ();
        }

        /// <summary>Adds a new data store provider to be managed.</summary>
        /// <param name="dataStoreType">The type of the provider.</param>
        /// <param name="options">The options to create a provider instance.</param>
        /// <param name="lifetime">The lifetime of the provider.</param>
        public void AddDataStore(Type dataStoreType, ProviderOptions options, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            => RegisteredProviders.Add(
                ProviderDescriptor.Describe(dataStoreType, options, lifetime)
            );
    }
}
