using System;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.AzureBlob
{
    /// <summary>A collection of extension methods to simplify the usage of Aws ProviderOptionsBuilder.</summary>
    [PublicAPI]
    public static class ProviderOptionsBuilderExtensions
    {
        /// <summary>Configures the Service Endpoint (connection string) to be used.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Azure Blob Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Service Endpoint on.</param>
        /// <param name="serviceEndpoint">The connection string for the Azure Blob service.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Credentials set.</returns>
        public static TOptionsBuilder UseServiceEndpoint<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [CanBeNull] string serviceEndpoint)
            where TOptionsBuilder : IProviderOptionsBuilder<AzureBlobProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetOption(nameof(AzureBlobProviderOptions.ConnectionString), serviceEndpoint);
            return builder;
        }

        /// <summary>Configures the Container to be used.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Azure Blob Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Service Endpoint on.</param>
        /// <param name="containerName">The name of the container to be managed.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Credentials set.</returns>
        public static TOptionsBuilder UseContainer<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [CanBeNull] string containerName)
            where TOptionsBuilder : IProviderOptionsBuilder<AzureBlobProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetOption(nameof(AzureBlobProviderOptions.ContainerName), containerName);
            return builder;
        }
    }
}
