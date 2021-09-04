using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataStoreFramework.Data;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A collection of extensions to simplify the usage of a Data Store Orchestrator.</summary>
    /// <remarks>These methods are all the <see cref="Providers.IDataStoreProvider"/> methods that the Orchestrator can forward without intervention.</remarks>
    [PublicAPI]
    public static class DataStoreOrchestratorExtensions
    {
        /// <summary>Retrieves the metadata for a give data object.</summary>
        /// <typeparam name="TOrchestrator">The specific implementation of <see cref="IDataStoreOrchestrator"/>.</typeparam>
        /// <param name="orchestrator">An instance of the specific Orchestrator.</param>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that will provide an instance of <see cref="ObjectMetadata"/> with metadata about the requested data object and it's versions.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [Pure]
        [NotNull]
        [MustUseReturnValue("Use the return value to describe the data object.")]
        public static Task<ObjectMetadata> GetMetadataAsync<TOrchestrator>(
            [NotNull] this TOrchestrator orchestrator,
            [NotNull] string objectPath,
            CancellationToken cancellationToken = default)
            where TOrchestrator : IDataStoreOrchestrator
        => orchestrator
                .GetProviderByObjectPath(objectPath)
                .GetMetadataAsync(objectPath, cancellationToken);

        /// <summary>Determines if a given <paramref name="objectPath"/> exists within the Provider's Data Store.</summary>
        /// <typeparam name="TOrchestrator">The specific implementation of <see cref="IDataStoreOrchestrator"/>.</typeparam>
        /// <param name="orchestrator">An instance of the specific Orchestrator.</param>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that returns <see langword="true"/> if the data object exists in the Provider's Data Store, otherwise <see langword="false"/>.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        public static Task<bool> ExistsAsync<TOrchestrator>(
            [NotNull] this TOrchestrator orchestrator,
            [NotNull] string objectPath,
            CancellationToken cancellationToken = default)
            where TOrchestrator : IDataStoreOrchestrator
        => orchestrator
                .GetProviderByObjectPath(objectPath)
                .ExistsAsync(objectPath, cancellationToken);

        /// <summary>Reads the content of a requests data object.</summary>
        /// <typeparam name="TOrchestrator">The specific implementation of <see cref="IDataStoreOrchestrator"/>.</typeparam>
        /// <param name="orchestrator">An instance of the specific Orchestrator.</param>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task that will provide a stream containing the contents of the requested data object.</returns>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        [NotNull]
        public static Task<Stream> ReadAsync<TOrchestrator>(
            [NotNull] this TOrchestrator orchestrator,
            [NotNull] string objectPath,
            CancellationToken cancellationToken = default)
            where TOrchestrator : IDataStoreOrchestrator
        => orchestrator
                .GetProviderByObjectPath(objectPath)
                .ReadAsync(objectPath, cancellationToken);

        /// <summary>Writes the content of <paramref name="data"/> the the specified <paramref name="objectPath"/>.</summary>
        /// <typeparam name="TOrchestrator">The specific implementation of <see cref="IDataStoreOrchestrator"/>.</typeparam>
        /// <param name="orchestrator">An instance of the specific Orchestrator.</param>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="data">A stream containing the contents of the data object.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        public static Task WriteAsync<TOrchestrator>(
            [NotNull] this TOrchestrator orchestrator,
            [NotNull] string objectPath,
            [NotNull] Stream data,
            CancellationToken cancellationToken = default)
            where TOrchestrator : IDataStoreOrchestrator
        => orchestrator
                .GetProviderByObjectPath(objectPath)
                .WriteAsync(objectPath, data, cancellationToken);

        /// <summary>Deletes a given data object (or data object version) from the Provider's Data Store.</summary>
        /// <typeparam name="TOrchestrator">The specific implementation of <see cref="IDataStoreOrchestrator"/>.</typeparam>
        /// <param name="orchestrator">An instance of the specific Orchestrator.</param>
        /// <param name="objectPath">An object path that specifies where the required data object is located.</param>
        /// <param name="versionId">A specific version identifier to be deleted from the data object history.</param>
        /// <param name="cancellationToken">A cancellation token for asynchronous operations.</param>
        /// <returns>An asynchronous task.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectNotFoundException">Thrown when the specified <paramref name="objectPath"/> doesn't exist.</exception>
        /// <exception cref="DataStoreFramework.Exceptions.ObjectException">Thrown when the provider is in a disconnected state.</exception>
        public static Task DeleteAsync<TOrchestrator>(
            [NotNull] this TOrchestrator orchestrator,
            [NotNull] string objectPath,
            [CanBeNull] string versionId = null,
            CancellationToken cancellationToken = default)
            where TOrchestrator : IDataStoreOrchestrator
        => orchestrator
                .GetProviderByObjectPath(objectPath)
                .DeleteAsync(objectPath, versionId, cancellationToken);
    }
}
