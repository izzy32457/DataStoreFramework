using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A collection of extensions to provide static data store orchestration configuration.</summary>
    [PublicAPI]
    public static class DataStoreOrchestratorBuilderExtensions
    {
        /// <summary>Configures a static configuration of data stores.</summary>
        /// <param name="builder">The Orchestrator builder.</param>
        /// <param name="configAction">An action to add providers to the static configuration implementation.</param>
        /// <returns>The instance of <paramref name="builder"/> with the static configuration provider set.</returns>
        public static DataStoreOrchestratorBuilder UseStaticConfiguration(this DataStoreOrchestratorBuilder builder, Action<IOrchestratorConfigurationProviderBuilder> configAction)
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
