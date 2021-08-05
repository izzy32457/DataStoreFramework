using System;
using System.Collections.Generic;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A collection of configuration provider builder extensions.</summary>
    [PublicAPI]
    public static class OrchestratorConfigurationProviderBuilderExtensions
    {
        private static readonly Dictionary<string, Type> ProviderTypes = new ();

        /// <summary>Adds a Data Store Provider to the Data Store Orchestrator.</summary>
        /// <typeparam name="TDataStoreOptions">The type of the data store provider options.</typeparam>
        /// <param name="configurationBuilder">A Data Store Orchestrator configuration builder.</param>
        /// <param name="config">A configuration to retrieve values from.</param>
        /// <returns>The passed in <paramref name="configurationBuilder"/> with the Data Store Provider registered.</returns>
        [NotNull]
        public static IOrchestratorConfigurationProviderBuilder FromConfiguration<TDataStoreOptions>(
            [NotNull] this IOrchestratorConfigurationProviderBuilder configurationBuilder,
            [NotNull] IConfiguration config)
            where TDataStoreOptions : ProviderOptions, new()
        {
            if (configurationBuilder is null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            var staticConfig = new List<StaticDataStoreConfiguration>();
            config.Bind(staticConfig);

            foreach (var storeConfiguration in staticConfig)
            {
                var providerType = GetProviderTypeByName(storeConfiguration.FullyQualifiedTypeName);
                var options = Activator.CreateInstance(typeof(TDataStoreOptions), storeConfiguration.Options) as TDataStoreOptions
                              ?? throw new InvalidOperationException("Unable to map options for ");

                configurationBuilder.AddDataStore(providerType, options);
            }

            return configurationBuilder;
        }

        [NotNull]
        private static Type GetProviderTypeByName([NotNull] string name)
        {
            if (ProviderTypes.ContainsKey(name))
            {
                return ProviderTypes[name];
            }

            var tmp = Type.GetType(name, false);
            // ReSharper disable once InvertIf
            if (tmp is not null)
            {
                ProviderTypes.Add(name, tmp);
                return tmp;
            }

            // TODO: implement discovery by IDataStoreProvider.Type name
            throw new NotSupportedException($"Provider name is unsupported: {name}");
        }
    }
}
