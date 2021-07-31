using System;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3
{
    /// <summary>A collection of extension methods for AWS S3 Provider Options.</summary>
    [PublicAPI]
    public static class AwsS3ProviderOptionsExtensions
    {
        /// <summary>Creates an <see cref="AmazonS3Client"/> instance from the specified options.</summary>
        /// <param name="options">The options for the AWS S3 provider.</param>
        /// <returns>An instance of <see cref="AmazonS3Client"/> configured with the specified <paramref name="options"/>.</returns>
        [NotNull]
        public static AmazonS3Client GetClient([NotNull] this AwsS3ProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var config = new AmazonS3Config
            {
                ForcePathStyle = options.ForcePathStyle,
            };

            if (options.ServiceEndpoint is not null)
            {
                config.ServiceURL = options.ServiceEndpoint;

                var endpointUrl = new Uri(options.ServiceEndpoint);
                config.UseHttp = endpointUrl.Scheme == Uri.UriSchemeHttp;
            }

            // region must be set after ServiceURL as setting that will null the region setting.
            if (options.Region is not null)
            {
                config.RegionEndpoint = options.Region;
            }

            AWSCredentials awsCredentials;
            if (options.HasProfile())
            {
                var chain = new CredentialProfileStoreChain();
                chain.TryGetAWSCredentials(options.Profile, out awsCredentials);
            }
            else if (options.HasBasicCredentials())
            {
                awsCredentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);
            }
            else
            {
                awsCredentials = FallbackCredentialsFactory.GetCredentials();
            }

            return new AmazonS3Client(awsCredentials, config);
        }

        /// <summary>Determines if a Profile has been defined for the given options instance.</summary>
        /// <param name="options">The options for the AWS S3 provider.</param>
        /// <returns><see langword="true"/> if a profile has been specified, otherwise <see langword="false"/>.</returns>
        public static bool HasProfile([NotNull] this AwsS3ProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.HasOption(nameof(options.Profile)) && !string.IsNullOrEmpty(options.Profile);
        }

        /// <summary>Determines if basic application credentials have been defined for the given options instance.</summary>
        /// <param name="options">The options for the AWS S3 provider.</param>
        /// <returns><see langword="true"/> if basic credentials have been specified, otherwise <see langword="false"/>.</returns>
        public static bool HasBasicCredentials([NotNull] this AwsS3ProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return !string.IsNullOrEmpty(options.AccessKey) &&
                   !string.IsNullOrEmpty(options.SecretKey);
        }
    }
}
