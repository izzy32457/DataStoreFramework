using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataStoreFramework.Data;
using DataStoreFramework.Orchestration.Exceptions;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A orchestrated data store provider to wrap a registered Data Store Orchestrator.</summary>
    [PublicAPI]
    public class OrchestratedDataStoreProvider : IDataStoreProvider
    {
        private readonly IDataStoreOrchestrator _dataStoreOrchestrator;

        /// <summary>Initializes a new instance of the <see cref="OrchestratedDataStoreProvider"/> class.</summary>
        /// <param name="orchestrator">An instance of a Data Store Orchestrator.</param>
        public OrchestratedDataStoreProvider(IDataStoreOrchestrator orchestrator)
        {
            _dataStoreOrchestrator = orchestrator;
        }

        /// <inheritdoc/>
        public string Type => "orchestrated";

        /// <inheritdoc/>
        public bool CanAccessObject(string objectPath)
        {
            try
            {
                var provider = _dataStoreOrchestrator.GetProviderByObjectPath(objectPath);
                return provider.CanAccessObject(objectPath);
            }
            catch (ProviderNotFoundException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public Task CopyAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.CopyAsync(sourceObjectPath, destinationObjectPath, cancellationToken);

        /// <inheritdoc/>
        public Task DeleteAsync(string objectPath, string versionId = null, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.DeleteAsync(objectPath, versionId, cancellationToken: cancellationToken);

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(string objectPath, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.ExistsAsync(objectPath, cancellationToken);

        /// <inheritdoc/>
        public Task<ObjectMetadata> GetMetadataAsync(string objectPath, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.GetMetadataAsync(objectPath, cancellationToken: cancellationToken);

        /// <inheritdoc/>
        public Task MoveAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.MoveAsync(sourceObjectPath, destinationObjectPath, cancellationToken);

        /// <inheritdoc/>
        public Task<Stream> ReadAsync(string objectPath, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.ReadAsync(objectPath, cancellationToken);

        /// <inheritdoc/>
        public Task WriteAsync(string objectPath, Stream data, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.WriteAsync(objectPath, data, cancellationToken);

        /// <inheritdoc/>
        public Task<string> StartChunkedWriteAsync(string objectPath, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.StartChunkedWriteAsync(objectPath, cancellationToken);

        /// <inheritdoc/>
        public Task<string> WriteChunkAsync(string chunkedUploadId, Stream chunkData, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.WriteChunkAsync(chunkedUploadId, chunkData, cancellationToken);

        /// <inheritdoc/>
        public Task EndChunkedWriteAsync(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.EndChunkedWriteAsync(chunkedUploadId, chunkDetails, cancellationToken);

        /// <inheritdoc/>
        public Task CancelChunkedWriteAsync(string chunkedUploadId, CancellationToken cancellationToken = default)
            => _dataStoreOrchestrator.CancelChunkedWriteAsync(chunkedUploadId, cancellationToken);
    }
}
