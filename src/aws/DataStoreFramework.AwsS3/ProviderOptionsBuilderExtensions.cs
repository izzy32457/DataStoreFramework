using System;
using Amazon;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3
{
    /// <summary>A collection of extension methods to simplify the usage of Aws ProviderOptionsBuilder.</summary>
    [PublicAPI]
    public static class ProviderOptionsBuilderExtensions
    {
        /// <summary>Configures the required AWS Region.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Aws S3 Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Region on.</param>
        /// <param name="region">The AWS defined Region name.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Region set.</returns>
        [NotNull]
        public static TOptionsBuilder UseRegion<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [NotNull] RegionEndpoint region)
            where TOptionsBuilder : IProviderOptionsBuilder<AwsS3ProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (region is null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            builder.SetOption(nameof(AwsS3ProviderOptions.Region), region);
            return builder;
        }

        /// <summary>Configures the required AWS Region.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Aws S3 Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Region on.</param>
        /// <param name="region">The AWS defined Region name.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Region set.</returns>
        [NotNull]
        public static TOptionsBuilder UseRegion<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [NotNull] string region)
            where TOptionsBuilder : IProviderOptionsBuilder<AwsS3ProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (region is null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            builder.SetOption(nameof(AwsS3ProviderOptions.Region), RegionEndpoint.GetBySystemName(region));
            return builder;
        }

        /// <summary>Configures the required Application Credentials.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Aws S3 Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Region on.</param>
        /// <param name="applicationKey">The application key.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Application Credentials set.</returns>
        public static TOptionsBuilder UseAppCredentials<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [NotNull] string applicationKey,
            [NotNull] string secretKey)
            where TOptionsBuilder : IProviderOptionsBuilder<AwsS3ProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (applicationKey is null)
            {
                throw new ArgumentNullException(nameof(applicationKey));
            }

            if (secretKey is null)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            builder.SetOption(nameof(AwsS3ProviderOptions.AccessKey), applicationKey);
            builder.SetOption(nameof(AwsS3ProviderOptions.SecretKey), secretKey);
            return builder;
        }

        /// <summary>Configures the Credentials Profile to be used.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Aws S3 Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Region on.</param>
        /// <param name="profileName">The profile name to be used.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Credentials set.</returns>
        /// <remarks>This requires the Profile to be configured for the AWS CLI.</remarks>
        public static TOptionsBuilder UseProfile<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [NotNull] string profileName)
            where TOptionsBuilder : IProviderOptionsBuilder<AwsS3ProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (profileName is null)
            {
                throw new ArgumentNullException(nameof(profileName));
            }

            builder.SetOption(nameof(AwsS3ProviderOptions.Profile), profileName);
            return builder;
        }

        /// <summary>Configures the Credentials Profile to be used.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Aws S3 Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Region on.</param>
        /// <param name="enforcePathStyle">Specifies if S3 service should enforce the use of path styles for it's API calls.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Credentials set.</returns>
        /// <remarks>This requires the Profile to be configured for the AWS CLI.</remarks>
        public static TOptionsBuilder ForcePathStyle<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            bool enforcePathStyle = true)
            where TOptionsBuilder : IProviderOptionsBuilder<AwsS3ProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetOption(nameof(AwsS3ProviderOptions.ForcePathStyle), enforcePathStyle);
            return builder;
        }

        /// <summary>Configures the Service Endpoint to be used.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Aws S3 Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Service Endpoint on.</param>
        /// <param name="serviceEndpoint">The endpoint URL for the AWS S3 service.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Credentials set.</returns>
        /// <remarks>
        /// For real AWS SDK usage either don't set this value or set it to <see langword="null"/>.
        /// For alternative local S3 implementations (e.g. MinIO or Localstack), set this value to the base URL of the service (including required port).
        /// </remarks>
        public static TOptionsBuilder UseServiceEndpoint<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [CanBeNull] string serviceEndpoint)
            where TOptionsBuilder : IProviderOptionsBuilder<AwsS3ProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetOption(nameof(AwsS3ProviderOptions.ServiceEndpoint), serviceEndpoint);
            return builder;
        }

        /// <summary>Configures the Service Endpoint to use known localstack configuration.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Aws S3 Provider Options Builder.</typeparam>
        /// <param name="builder">The Provider Options Builder to configure the Service Endpoint on.</param>
        /// <param name="serviceDomain">The domain that localstack is accessible from.</param>
        /// <param name="servicePort">The port number that localstack is accessible on.</param>
        /// <returns>The passed in <paramref name="builder"/> with the Credentials set.</returns>
        public static TOptionsBuilder UseLocalstack<TOptionsBuilder>(
            [NotNull] this TOptionsBuilder builder,
            [NotNull] string serviceDomain = "localhost",
            ushort servicePort = 4566)
            where TOptionsBuilder : IProviderOptionsBuilder<AwsS3ProviderOptions>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetOption(nameof(AwsS3ProviderOptions.ServiceEndpoint), $"http://{serviceDomain}:{servicePort}/");
            builder.SetOption(nameof(AwsS3ProviderOptions.ForcePathStyle), true);
            return builder;
        }
    }
}
