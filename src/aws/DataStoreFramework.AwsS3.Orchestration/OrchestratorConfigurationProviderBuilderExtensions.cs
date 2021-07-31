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
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to create an <see cref="AwsS3ProviderOptions"/> instance.</param>
        /// <returns>The passed in <paramref name="services"/> with the AWS S3 Data Store Provider registered.</returns>
        [NotNull]
        public static IOrchestratorConfigurationProviderBuilder AddAwsS3DataStore(
            [NotNull] this IOrchestratorConfigurationProviderBuilder services,
            [CanBeNull] Action<AwsS3ProviderOptionsBuilder> builder)
            => services.AddDataStore<AwsS3Provider, AwsS3ProviderOptions, AwsS3ProviderOptionsBuilder>(builder);
    }
}
