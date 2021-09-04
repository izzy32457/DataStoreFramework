using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A collection of extensions for <see cref="IConfiguration"/> to simplify Data Store Orchestrator configurations.</summary>
    [PublicAPI]
    public static class ConfigurationExtensions
    {
        /// <summary>Constructs an enumeration of <see cref="ProviderDescriptor"/> to register with an orchestrator with the options specified in the "DSF" section in the <see cref="IConfiguration"/> object.</summary>
        /// <param name="config">An <see cref="IConfiguration"/> instance.</param>
        /// <returns>An enumeration of <see cref="ProviderDescriptor"/> to register with an orchestrator.</returns>
        [NotNull]
        public static IEnumerable<ProviderDescriptor> GetOrchestrationOptions(this IConfiguration config)
            => GetOrchestrationOptions(config, DataStoreFramework.ConfigurationExtensions.DefaultConfigSection);

        /// <summary>Constructs a set of <see cref="ProviderDescriptor"/> to register with an orchestrator with the options specified in the provided in the <see cref="IConfiguration"/> object.</summary>
        /// <param name="config">An <see cref="IConfiguration"/> instance.</param>
        /// <param name="configSection">The config section to extract data store provider options from.</param>
        /// <returns>An enumeration of <see cref="ProviderDescriptor"/> to register with an orchestrator.</returns>
        [NotNull]
        public static IEnumerable<ProviderDescriptor> GetOrchestrationOptions(
            [NotNull] this IConfiguration config,
            [CanBeNull] string configSection)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var section = string.IsNullOrEmpty(configSection) ? config : config.GetSection(configSection);
            if (section is null)
            {
                throw new ArgumentNullException(nameof(config), "Could not locate required configuration section.");
            }

            // TODO: Update this to discover all Orchestrated Provider details
            throw new NotImplementedException();
        }
    }
}
