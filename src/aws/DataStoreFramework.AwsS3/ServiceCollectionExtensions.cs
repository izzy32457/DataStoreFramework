using System;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DataStoreFramework.AwsS3
{
    /// <summary>A collection of extension methods to simplify registering AwsS3 Data Store Provider with the dependency injection framework.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds an AWS S3 Data Store Provider to the dependency injection system.</summary>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to create an <see cref="AwsS3ProviderOptions"/> instance.</param>
        /// <returns>The passed in <paramref name="services"/> with the AWS S3 Data Store Provider registered.</returns>
        public static IServiceCollection AddAwsS3DataStore(this IServiceCollection services, Action<AwsS3ProviderOptionsBuilder> builder)
            => services.AddDataStore<AwsS3Provider, AwsS3ProviderOptions, AwsS3ProviderOptionsBuilder>(builder);

        /// <summary>Adds an AWS S3 Data Store Provider to the dependency injection system with the specified lifetime.</summary>
        /// <param name="services">A collection of service dependency registrations.</param>
        /// <param name="builder">A builder method to create an <see cref="AwsS3ProviderOptions"/> instance.</param>
        /// <param name="serviceLifetime">The required service lifetime for the Data Store Provider.</param>
        /// <returns>The passed in <paramref name="services"/> with the AWS S3 Data Store Provider registered.</returns>
        public static IServiceCollection AddAwsS3DataStore(this IServiceCollection services, Action<AwsS3ProviderOptionsBuilder> builder, ServiceLifetime serviceLifetime)
            => services.AddDataStore<AwsS3Provider, AwsS3ProviderOptions, AwsS3ProviderOptionsBuilder>(builder, serviceLifetime);
    }
}
