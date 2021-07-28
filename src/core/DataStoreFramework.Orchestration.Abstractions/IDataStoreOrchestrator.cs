using System.Collections.Generic;
using System.IO;
using DataStoreFramework.Data;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>Represents a Data Store Provider Orchestrator.</summary>
    /// <remarks>Most methods will be forwarded to the relevant provider unless multiple providers are required (e.g. Copy or Move).</remarks>
    [PublicAPI]
    public interface IDataStoreOrchestrator
    {
        /// <summary>Retrieves an instance of a registered provider by it's registered identifier.</summary>
        /// <param name="name">The name of the required provider.</param>
        /// <returns>An instance of the specified Data Store Provider.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found with the specified <paramref name="name"/>.</exception>
        [NotNull]
        IDataStoreProvider GetProviderByName([NotNull] string name);

        /// <summary>Retrieves an instance of a registered provider by it's registered identifier.</summary>
        /// <param name="objectPath">The path for an object stored in the required provider.</param>
        /// <returns>An instance of the specified Data Store Provider.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        [NotNull]
        IDataStoreProvider GetProviderByObjectPath([NotNull] string objectPath);

        /// <summary>Begins a chunked data upload.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <returns>An identifier to enable uploading multiple chunks.</returns>
        /// <remarks>This process should be used to ensure data uploads for large content will not timeout or fail due to request size limitations.</remarks>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        [MustUseReturnValue("Use the return value to identify the started chunked upload.")]
        string StartChunkedWrite([NotNull] string objectPath);

        /// <summary>Writes a new data part to the specified chunked upload identifier.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWrite"/>.</param>
        /// <param name="chunkData">A stream containing the contents of the data object part.</param>
        /// <returns>An identifier for the uploaded data object part for validation checks when calling <see cref="EndChunkedWrite"/>.</returns>
        /// <exception cref="Exceptions.OrchestrationException">Thrown if the <paramref name="chunkedUploadId"/> is not being tracked by the Orchestrator.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        string WriteChunk([NotNull] string chunkedUploadId, [NotNull] Stream chunkData);

        /// <summary>Completes a chunked data upload session and validates all parts have been received successfully.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWrite"/>.</param>
        /// <param name="chunkDetails">A set of data part details that include the identifiers provided by calls to <see cref="WriteChunk"/> and the relevant data hash values to compare.</param>
        /// <exception cref="Exceptions.OrchestrationException">Thrown if the <paramref name="chunkedUploadId"/> is not being tracked by the Orchestrator.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void EndChunkedWrite([NotNull] string chunkedUploadId, [NotNull][ItemNotNull][InstantHandle] IEnumerable<ChunkDetail> chunkDetails);

        /// <summary>Completes a chunked data upload and removes all parts currently uploaded.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWrite"/>.</param>
        /// <exception cref="Exceptions.OrchestrationException">Thrown if the <paramref name="chunkedUploadId"/> is not being tracked by the Orchestrator.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void CancelChunkedWrite([NotNull] string chunkedUploadId);

        /// <summary>Copies a given data object from one location in to another location within the same Provider / Data Store.</summary>
        /// <param name="sourceObjectPath">An object path that specifies where the source data object is located.</param>
        /// <param name="destinationObjectPath">An object path that defines where the data object should be copied to.</param>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown when the provider for the specified <paramref name="sourceObjectPath"/> or <paramref name="destinationObjectPath"/> doesn't exist.</exception>
        void Copy([NotNull] string sourceObjectPath, [NotNull] string destinationObjectPath);

        /// <summary>Moves a given data object from one location to another within the same Provider / Data Store.</summary>
        /// <param name="sourceObjectPath">An object path that specifies where the source data object is located.</param>
        /// <param name="destinationObjectPath">An object path that defines where the data object should be moved to.</param>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown when the provider for the specified <paramref name="sourceObjectPath"/> or <paramref name="destinationObjectPath"/> doesn't exist.</exception>
        void Move([NotNull] string sourceObjectPath, [NotNull] string destinationObjectPath);
    }
}
