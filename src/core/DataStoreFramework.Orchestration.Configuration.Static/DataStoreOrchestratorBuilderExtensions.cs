using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A collection of extensions to provide static data store orchestration configuration.</summary>
    [PublicAPI]
    public static class DataStoreOrchestratorBuilderExtensions
    {
        /// <summary>Configures a static configuration of data stores.</summary>
        /// <param name="builder">The Orchestrator builder.</param>
        /// <param name="config">A configuration provider instance.</param>
        /// <param name="configSection">The config section to extract data store provider options from.</param>
        /// <returns>The instance of <paramref name="builder"/> with the static configuration provider set.</returns>
        [NotNull]
        public static DataStoreOrchestratorBuilder UseStaticConfiguration(
            [NotNull] this DataStoreOrchestratorBuilder builder,
            [NotNull] IConfiguration config,
            [CanBeNull] string configSection = null)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return builder.UseStaticConfiguration(opts =>
            {
                // Discover providers and register them
                var providers = config.GetOrchestrationOptions(configSection);
                opts.AddDataStoreRange(providers);
            });
        }

        /// <summary>Configures a static configuration of data stores.</summary>
        /// <param name="builder">The Orchestrator builder.</param>
        /// <param name="configAction">An action to add providers to the static configuration implementation.</param>
        /// <returns>The instance of <paramref name="builder"/> with the static configuration provider set.</returns>
        [NotNull]
        public static DataStoreOrchestratorBuilder UseStaticConfiguration(
            [NotNull] this DataStoreOrchestratorBuilder builder,
            [CanBeNull] Action<IOrchestratorConfigurationProviderBuilder<OrchestratorStaticConfigurationProvider>> configAction = null)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var configProvider = new OrchestratorStaticConfigurationProviderBuilder();
            configAction?.Invoke(configProvider);
            builder.ConfigurationProvider = configProvider.Build();

            return builder;
        }
    }
}
