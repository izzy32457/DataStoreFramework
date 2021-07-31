using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A collection of extensions to enable registering the Data Store Orchestrator with Dependency Injection framework.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds a Data Store Orchestrator to the Dependency Injection services.</summary>
        /// <param name="services">A collection of services.</param>
        /// <param name="configAction">A function to configure the Data Store Orchestrator.</param>
        /// <returns>The instance of <paramref name="services"/> passed in with the Orchestrator registered.</returns>
        public static IServiceCollection AddDataStoreOrchestration(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<DataStoreOrchestratorBuilder> configAction)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configAction is null)
            {
                throw new ArgumentNullException(nameof(configAction));
            }

            var builder = new DataStoreOrchestratorBuilder();
            configAction(builder);

            return services
                .AddSingleton(builder.ConfigurationProvider)
                .AddSingleton<DataStoreOrchestrator>()
                .AddSingleton<IDataStoreOrchestrator>(sp => sp.GetRequiredService<DataStoreOrchestrator>())
                ;
        }
    }
}
