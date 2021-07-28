using System.Collections.Generic;
using System.IO;
using DataStoreFramework.Data;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DataStoreFramework.AwsS3
{
    public class AwsS3Provider : IDataStoreProvider
    {
        private readonly ILogger<AwsS3Provider> _logger;

        private readonly AwsS3ProviderOptions _options;

        public AwsS3Provider(ILogger<AwsS3Provider> logger, AwsS3ProviderOptions options)
        {
            _logger = logger;
            _options = options;
        }

        /// <inheritdoc/>
        public string Type => "s3";

        /// <inheritdoc/>
        public bool CanAccessObject(string objectPath)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void CancelChunkedWrite(string chunkedUploadId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void Copy(string sourceObjectPath, string destinationObjectPath)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void Delete(string objectPath, string versionId = null)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void EndChunkedWrite(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Exists(string objectPath)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public ObjectMetadata GetMetadata(string objectPath)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void Move(string sourceObjectPath, string destinationObjectPath)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Stream Read(string objectPath)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public string StartChunkedWrite(string objectPath)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void Write(string objectPath, Stream data)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public string WriteChunk(string chunkedUploadId, Stream chunkData)
        {
            throw new System.NotImplementedException();
        }
    }
}
