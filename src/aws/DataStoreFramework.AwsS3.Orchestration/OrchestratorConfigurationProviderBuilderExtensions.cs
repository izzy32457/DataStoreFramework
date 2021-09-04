using System;
using DataStoreFramework.Orchestration;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3.Orchestration
{
    /// <summary>A collection of extensions to configure the AWS S3 Provider in the Data Store Orchestrator.</summary>
    [PublicAPI]
    public static class OrchestratorConfigurationProviderBuilderExtensions
    {
        /// <summary>Adds an AWS S3 Data Store Provider to the dependency injection system.</summary>
        /// <typeparam name="TOrchestratorConfigurationProvider">The orchestrator configuration provider type the builder will create.</typeparam>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to create an <see cref="AwsS3ProviderOptions"/> instance.</param>
        /// <returns>The passed in <paramref name="services"/> with the AWS S3 Data Store Provider registered.</returns>
        [NotNull]
        public static IOrchestratorConfigurationProviderBuilder<TOrchestratorConfigurationProvider> AddAwsS3DataStore<TOrchestratorConfigurationProvider>(
            [NotNull] this IOrchestratorConfigurationProviderBuilder<TOrchestratorConfigurationProvider> services,
            [CanBeNull] Action<AwsS3ProviderOptionsBuilder> builder)
            where TOrchestratorConfigurationProvider : IOrchestratorConfigurationProvider
            => services.AddDataStore<TOrchestratorConfigurationProvider, AwsS3Provider, AwsS3ProviderOptions, AwsS3ProviderOptionsBuilder>(builder);
    }
}
