using System.Collections.Generic;
using System.IO;
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
        public void Copy(string sourceObjectPath, string destinationObjectPath)
            => _dataStoreOrchestrator.Copy(sourceObjectPath, destinationObjectPath);

        /// <inheritdoc/>
        public void Delete(string objectPath, string versionId = null)
            => _dataStoreOrchestrator.Delete(objectPath, versionId);

        /// <inheritdoc/>
        public bool Exists(string objectPath)
            => _dataStoreOrchestrator.Exists(objectPath);

        /// <inheritdoc/>
        public ObjectMetadata GetMetadata(string objectPath)
            => _dataStoreOrchestrator.GetMetadata(objectPath);

        /// <inheritdoc/>
        public void Move(string sourceObjectPath, string destinationObjectPath)
            => _dataStoreOrchestrator.Move(sourceObjectPath, destinationObjectPath);

        /// <inheritdoc/>
        public Stream Read(string objectPath)
            => _dataStoreOrchestrator.Read(objectPath);

        /// <inheritdoc/>
        public void Write(string objectPath, Stream data)
            => _dataStoreOrchestrator.Write(objectPath, data);

        /// <inheritdoc/>
        public string StartChunkedWrite(string objectPath)
            => _dataStoreOrchestrator.StartChunkedWrite(objectPath);

        /// <inheritdoc/>
        public string WriteChunk(string chunkedUploadId, Stream chunkData)
            => _dataStoreOrchestrator.WriteChunk(chunkedUploadId, chunkData);

        /// <inheritdoc/>
        public void EndChunkedWrite(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails)
            => _dataStoreOrchestrator.EndChunkedWrite(chunkedUploadId, chunkDetails);

        /// <inheritdoc/>
        public void CancelChunkedWrite(string chunkedUploadId)
            => _dataStoreOrchestrator.CancelChunkedWrite(chunkedUploadId);
    }
}
