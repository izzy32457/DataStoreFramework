using System;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.AzureBlob
{
    /// <summary>A collection of extension methods to simplify registering Azure Blob Data Store Provider with the dependency injection framework.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds an Azure Blob Data Store Provider to the dependency injection system.</summary>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to create an <see cref="AzureBlobProviderOptions"/> instance.</param>
        /// <returns>The passed in <paramref name="services"/> with the Azure Blob Data Store Provider registered.</returns>
        public static IServiceCollection AddAzureBlobDataStore(this IServiceCollection services, Action<AzureBlobProviderOptionsBuilder> builder)
            => services.AddDataStore<AzureBlobProvider, AzureBlobProviderOptions, AzureBlobProviderOptionsBuilder>(builder);

        /// <summary>Adds an Azure Blob Data Store Provider to the dependency injection system with the specified lifetime.</summary>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to create an <see cref="AzureBlobProviderOptions"/> instance.</param>
        /// <param name="serviceLifetime">The required service lifetime for the Data Store Provider.</param>
        /// <returns>The passed in <paramref name="services"/> with the Azure Blob Data Store Provider registered.</returns>
        public static IServiceCollection AddAzureBlobDataStore(this IServiceCollection services, Action<AzureBlobProviderOptionsBuilder> builder, ServiceLifetime serviceLifetime)
            => services.AddDataStore<AzureBlobProvider, AzureBlobProviderOptions, AzureBlobProviderOptionsBuilder>(builder, serviceLifetime);
    }
}
