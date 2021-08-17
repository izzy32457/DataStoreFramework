using System;
using System.Collections.Generic;
using System.IO;
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
        public void CancelChunkedWrite(string chunkedUploadId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Copy(string sourceObjectPath, string destinationObjectPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Delete(string objectPath, string versionId = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void EndChunkedWrite(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Exists(string objectPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ObjectMetadata GetMetadata(string objectPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Move(string sourceObjectPath, string destinationObjectPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Stream Read(string objectPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string StartChunkedWrite(string objectPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Write(string objectPath, Stream data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string WriteChunk(string chunkedUploadId, Stream chunkData)
        {
            throw new NotImplementedException();
        }
    }
}
