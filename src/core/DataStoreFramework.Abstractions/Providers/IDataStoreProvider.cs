using System.Collections.Generic;
using System.IO;
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
        /// <returns>An instance of <see cref="ObjectMetadata"/> with metadata about the requested data object and it's versions.</returns>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [Pure]
        [NotNull]
        [MustUseReturnValue("Use the return value to describe the data object.")]
        ObjectMetadata GetMetadata([NotNull] string objectPath);

        /// <summary>Determines if a given <paramref name="objectPath"/> exists within the Provider's Data Store.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <returns><see langword="true"/> if the data object exists in the Provider's Data Store, otherwise <see langword="false"/>.</returns>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        bool Exists([NotNull] string objectPath);

        /// <summary>Reads the content of a requests data object.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <returns>A stream containing the contents of the requested data object.</returns>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        Stream Read([NotNull] string objectPath);

        /// <summary>Writes the content of <paramref name="data"/> the the specified <paramref name="objectPath"/>.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="data">A stream containing the contents of the data object.</param>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void Write([NotNull] string objectPath, [NotNull] Stream data);

        /// <summary>Begins as chunked data upload.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <returns>An identifier to enable uploading multiple chunks.</returns>
        /// <remarks>This process should be used to ensure data uploads for large content will not timeout or fail due to request size limitations.</remarks>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        [MustUseReturnValue("Use the return value to identify the started chunked upload.")]
        string StartChunkedWrite([NotNull] string objectPath);

        /// <summary>Writes a new data part to the specified chunked upload identifier.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWrite"/>.</param>
        /// <param name="chunkData">A stream containing the contents of the data object part.</param>
        /// <returns>An identifier for the uploaded data object part for validation checks when calling <see cref="EndChunkedWrite"/>.</returns>
        /// <exception cref="Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        string WriteChunk([NotNull] string chunkedUploadId, [NotNull] Stream chunkData);

        /// <summary>Completes a chunked data upload session and validates all parts have been received successfully.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWrite"/>.</param>
        /// <param name="chunkDetails">A set of data part details that include the identifiers provided by calls to <see cref="WriteChunk"/> and the relevant data hash values to compare.</param>
        /// <exception cref="Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void EndChunkedWrite([NotNull] string chunkedUploadId, [NotNull][ItemNotNull][InstantHandle] IEnumerable<ChunkDetail> chunkDetails);

        /// <summary>Completes a chunked data upload and removes all parts currently uploaded.</summary>
        /// <param name="chunkedUploadId">The upload identifier provided by a call to <see cref="StartChunkedWrite"/>.</param>
        /// <exception cref="Exceptions.ObjectChunkedUploadException">Thrown when a chunked upload identifier is invalid.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void CancelChunkedWrite([NotNull] string chunkedUploadId);

        /// <summary>Deletes a given data object (or data object version) from the Provider's Data Store.</summary>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="versionId">A specific version identifier to be deleted from the data object history.</param>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void Delete([NotNull] string objectPath, [CanBeNull] string versionId = null);

        /// <summary>Copies a given data object from one location in to another location within the same Provider / Data Store.</summary>
        /// <param name="sourceObjectPath">An object path that specifies where the source data object is located.</param>
        /// <param name="destinationObjectPath">An object path that defines where the data object should be copied to.</param>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="sourceObjectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void Copy([NotNull] string sourceObjectPath, [NotNull] string destinationObjectPath);

        /// <summary>Moves a given data object from one location to another within the same Provider / Data Store.</summary>
        /// <param name="sourceObjectPath">An object path that specifies where the source data object is located.</param>
        /// <param name="destinationObjectPath">An object path that defines where the data object should be moved to.</param>
        /// <exception cref="Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="sourceObjectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        void Move([NotNull] string sourceObjectPath, [NotNull] string destinationObjectPath);
    }
}
