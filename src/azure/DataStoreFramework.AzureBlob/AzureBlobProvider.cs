using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataStoreFramework.Data;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DataStoreFramework.AzureBlob
{
    /// <summary>Provides the Data Store Provider implementation for Azure Blob Storage.</summary>
    [PublicAPI]
    public class AzureBlobProvider : IDataStoreProvider
    {
        [NotNull]
        private readonly ILogger<AzureBlobProvider> _logger;

        [NotNull]
        private readonly AzureBlobProviderOptions _options;

        /// <summary>Initializes a new instance of the <see cref="AzureBlobProvider"/> class.</summary>
        /// <param name="logger">A logger.</param>
        /// <param name="options">A set of options to configure the provider instance.</param>
        public AzureBlobProvider([NotNull] ILogger<AzureBlobProvider> logger, [NotNull] AzureBlobProviderOptions options)
        {
            _logger = logger;
            _options = options;
        }

        /// <inheritdoc/>
        public string Type => "azblob";

        /// <inheritdoc/>
        public bool CanAccessObject(string objectPath)
        {
            _logger.LogDebug($"Starting CanAccessObject: '{objectPath}'");

            var client = _options.GetClient();

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CancelChunkedWriteAsync(string chunkedUploadId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CopyAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string objectPath, string versionId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task EndChunkedWriteAsync(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ObjectMetadata> GetMetadataAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task MoveAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Stream> ReadAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> StartChunkedWriteAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task WriteAsync(string objectPath, Stream data, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> WriteChunkAsync(string chunkedUploadId, Stream chunkData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
