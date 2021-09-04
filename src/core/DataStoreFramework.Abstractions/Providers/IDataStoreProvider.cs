using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataStoreFramework.Data;
using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>Represents a Data Store Provider with methods to manage data held within the Store.</summary>
    [PublicAPI]
    public interface IDataStoreProvider
    {
        /// <summary>Gets the discriminator name for the Data Store Provider.</summary>
        /// <remarks>This is generally used as part of an object path to determine the correct provider type to use.</remarks>
        [NotNull]
        string Type { get; }

        /// <summary>Determines if the Data Store Provider can map the specified <paramref name="objectPath"/>.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <returns><see langword="true"/> if the provider can map the provided <paramref name="objectPath"/>, otherwise <see langword="false"/>.</returns>
        bool CanAccessObject([NotNull] string objectPath);

        /// <summary>Retrieves the metadata for a give data object.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that will provide an instance of <see cref="ObjectMetadata"/> with metadata about the requested data object and it's versions.</returns>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [Pure]
        [NotNull]
        [MustUseReturnValue("Use the return value to describe the data object.")]
        Task<ObjectMetadata> GetMetadataAsync([NotNull] string objectPath, CancellationToken cancellationToken = default);

        /// <summary>Determines if a given <paramref name="objectPath"/> exists within the Provider's Data Store.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that returns <see langword="true"/> if the data object exists in the Provider's Data Store, otherwise <see langword="false"/>.</returns>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        Task<bool> ExistsAsync([NotNull] string objectPath, CancellationToken cancellationToken = default);

        /// <summary>Reads the content of a requests data object.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that will provide a stream containing the contents of the requested data object.</returns>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        Task<Stream> ReadAsync([NotNull] string objectPath, CancellationToken cancellationToken = default);

        /// <summary>Writes the content of <paramref name="data"/> the the specified <paramref name="objectPath"/>.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="data">A stream containing the contents of the data object.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        Task WriteAsync([NotNull] string objectPath, [NotNull] Stream data, CancellationToken cancellationToken = default);

        /// <summary>Begins a chunked data upload.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that will provide an identifier to enable uploading multiple chunks.</returns>
        /// <remarks>This process should be used to ensure data uploads for large content will not timeout or fail due to request size limitations.</remarks>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        [MustUseReturnValue("Use the return value to identify the started chunked upload.")]
        Task<string> StartChunkedWriteAsync([NotNull] string objectPath, CancellationToken cancellationToken = default);

        /// <summary>Writes a new data part to the specified chunked upload identifier.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWriteAsync"/>.</param>
        /// <param name="chunkData">A stream containing the contents of the data object part.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that will provide an identifier for the uploaded data object part for validation checks when calling <see cref="EndChunkedWriteAsync"/>.</returns>
        /// <exception cref="Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        Task<string> WriteChunkAsync([NotNull] string chunkedUploadId, [NotNull] Stream chunkData, CancellationToken cancellationToken = default);

        /// <summary>Completes a chunked data upload session and validates all parts have been received successfully.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWriteAsync"/>.</param>
        /// <param name="chunkDetails">A set of data part details that include the identifiers provided by calls to <see cref="WriteChunkAsync"/> and the relevant data hash values to compare.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        Task EndChunkedWriteAsync([NotNull] string chunkedUploadId, [NotNull][ItemNotNull][InstantHandle] IEnumerable<ChunkDetail> chunkDetails, CancellationToken cancellationToken = default);

        /// <summary>Completes a chunked data upload and removes all parts currently uploaded.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWriteAsync"/>.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        Task CancelChunkedWriteAsync([NotNull] string chunkedUploadId, CancellationToken cancellationToken = default);

        /// <summary>Deletes a given data object (or data object version) from the Provider's Data Store.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="versionId">A specific version identifier to be deleted from the data object history.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        Task DeleteAsync([NotNull] string objectPath, [CanBeNull] string versionId = null, CancellationToken cancellationToken = default);

        /// <summary>Copies a given data object from one location in to another location within the same Provider / Data Store.</summary>
        /// <param name="sourceObjectPath">An object path that specifies where the source data object is located.</param>
        /// <param name="destinationObjectPath">An object path that defines where the data object should be copied to.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="sourceObjectPath"/> or <paramref name="destinationObjectPath"/> don't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        Task CopyAsync([NotNull] string sourceObjectPath, [NotNull] string destinationObjectPath, CancellationToken cancellationToken = default);

        /// <summary>Moves a given data object from one location to another within the same Provider / Data Store.</summary>
        /// <param name="sourceObjectPath">An object path that specifies where the source data object is located.</param>
        /// <param name="destinationObjectPath">An object path that defines where the data object should be moved to.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="sourceObjectPath"/> or <paramref name="destinationObjectPath"/> don't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        Task MoveAsync([NotNull] string sourceObjectPath, [NotNull] string destinationObjectPath, CancellationToken cancellationToken = default);
    }
}
