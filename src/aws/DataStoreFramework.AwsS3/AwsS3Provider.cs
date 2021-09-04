using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataStoreFramework.Data;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DataStoreFramework.AwsS3
{
    /// <summary>Provides the Data Store Provider implementation for AWS S3.</summary>
    [PublicAPI]
    public class AwsS3Provider : IDataStoreProvider
    {
        [NotNull]
        private readonly ILogger<AwsS3Provider> _logger;

        [NotNull]
        private readonly AwsS3ProviderOptions _options;

        /// <summary>Initializes a new instance of the <see cref="AwsS3Provider"/> class.</summary>
        /// <param name="logger">A logger.</param>
        /// <param name="options">A set of options to configure the provider instance.</param>
        public AwsS3Provider([NotNull] ILogger<AwsS3Provider> logger, [NotNull] AwsS3ProviderOptions options)
        {
            _logger = logger;
            _options = options;
        }

        /// <inheritdoc/>
        public string Type => "s3";

        /// <inheritdoc/>
        public bool CanAccessObject(string objectPath)
        {
            _logger.LogDebug($"Starting CanAccessObject: '{objectPath}'");

            using var client = _options.GetClient();

            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CancelChunkedWriteAsync(string chunkedUploadId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CopyAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string objectPath, string versionId = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task EndChunkedWriteAsync(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ObjectMetadata> GetMetadataAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task MoveAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Stream> ReadAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> StartChunkedWriteAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task WriteAsync(string objectPath, Stream data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> WriteChunkAsync(string chunkedUploadId, Stream chunkData, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
