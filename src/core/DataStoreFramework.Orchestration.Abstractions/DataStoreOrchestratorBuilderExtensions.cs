using System;

namespace DataStoreFramework.Orchestration
{
    public static class DataStoreOrchestratorBuilderExtensions
    {
        public static DataStoreOrchestratorBuilder UseStaticConfiguration(this DataStoreOrchestratorBuilder builder, Action<IOrchestratorConfigurationProvider> configAction)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var configProvider = new OrchestratorStaticConfigurationProvider();
            configAction?.Invoke(configProvider);
            builder.ConfigurationProvider = configProvider;

            return builder;
        }
    }
}
