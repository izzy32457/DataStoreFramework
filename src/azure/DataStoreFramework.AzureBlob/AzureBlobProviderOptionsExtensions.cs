using System;
using Azure.Storage.Blobs;
using JetBrains.Annotations;

namespace DataStoreFramework.AzureBlob
{
    /// <summary>A collection of extension methods for Azure Blob Provider Options.</summary>
    [PublicAPI]
    public static class AzureBlobProviderOptionsExtensions
    {
        /// <summary>Creates an <see cref="BlobContainerClient"/> instance from the specified options.</summary>
        /// <param name="options">The options for the AWS S3 provider.</param>
        /// <returns>An instance of <see cref="BlobContainerClient"/> configured with the specified <paramref name="options"/>.</returns>
        [NotNull]
        public static BlobContainerClient GetClient([NotNull] this AzureBlobProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var serviceUri = new Uri(options.ConnectionString);

            // TODO: Add Authentication here
            var client = new BlobServiceClient(serviceUri);

            return client.GetBlobContainerClient(options.ContainerName);
        }
    }
}
