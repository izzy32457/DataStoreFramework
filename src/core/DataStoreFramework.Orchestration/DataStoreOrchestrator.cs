using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataStoreFramework.Data;
using DataStoreFramework.Orchestration.Exceptions;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A default Orchestrator implementation.</summary>
    [PublicAPI]
    public class DataStoreOrchestrator : IDataStoreOrchestrator
    {
        private static readonly ConcurrentDictionary<string, IDataStoreProvider> ChunkedUploadsMap = new ();

        private readonly IOrchestratorConfigurationProvider _provider;

        private readonly ILogger<DataStoreOrchestrator> _logger;

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStoreOrchestrator"/> class.
        /// </summary>
        /// <param name="serviceProvider">A collection of registered services.</param>
        /// <param name="provider">A configuration provider for accessing registered Data Stores.</param>
        /// <param name="logger">A logger.</param>
        internal DataStoreOrchestrator(IServiceProvider serviceProvider, IOrchestratorConfigurationProvider provider, ILogger<DataStoreOrchestrator> logger)
        {
            _provider = provider;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public IDataStoreProvider GetProviderByName(string name)
            => _provider.GetDataStoreByName(name, _serviceProvider);

        /// <inheritdoc/>
        public IDataStoreProvider GetProviderByObjectPath(string objectPath)
            => _provider.GetDataStoreByObjectPath(objectPath, _serviceProvider);

        /// <inheritdoc/>
        public async Task CopyAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
        {
            var srcProvider = _provider.GetDataStoreByObjectPath(sourceObjectPath, _serviceProvider);
            var destProvider = _provider.GetDataStoreByObjectPath(destinationObjectPath, _serviceProvider);

            if (srcProvider is null)
            {
                throw new ProviderNotFoundException($"Unable to locate a Data Store Provider for '{sourceObjectPath}'");
            }

            if (destProvider is null)
            {
                throw new ProviderNotFoundException($"Unable to locate a Data Store Provider for '{destinationObjectPath}'");
            }

            if (ReferenceEquals(srcProvider, destProvider))
            {
                await srcProvider.CopyAsync(sourceObjectPath, destinationObjectPath, cancellationToken).ConfigureAwait(false);
#pragma warning disable SA1513 // Closing brace should be followed by blank line
            }
            //else if (srcProvider.Type == destProvider.Type)
            //{
            //    // TODO: How could we implement copy across accounts / buckets but within the same provider natively
            //}
            else
#pragma warning restore SA1513 // Closing brace should be followed by blank line
            {
                await using var stream = await srcProvider.ReadAsync(sourceObjectPath, cancellationToken).ConfigureAwait(false);
                await destProvider.WriteAsync(destinationObjectPath, stream, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task MoveAsync(string sourceObjectPath, string destinationObjectPath, CancellationToken cancellationToken = default)
        {
            var srcProvider = _provider.GetDataStoreByObjectPath(sourceObjectPath, _serviceProvider);
            var destProvider = _provider.GetDataStoreByObjectPath(destinationObjectPath, _serviceProvider);

            if (srcProvider is null)
            {
                throw new ProviderNotFoundException($"Unable to locate a Data Store Provider for '{sourceObjectPath}'");
            }

            if (destProvider is null)
            {
                throw new ProviderNotFoundException($"Unable to locate a Data Store Provider for '{destinationObjectPath}'");
            }

            if (ReferenceEquals(srcProvider, destProvider))
            {
                await srcProvider.MoveAsync(sourceObjectPath, destinationObjectPath, cancellationToken).ConfigureAwait(false);
#pragma warning disable SA1513 // Closing brace should be followed by blank line
            }
            //else if (srcProvider.Type == destProvider.Type)
            //{
            //    // TODO: How could we implement copy across accounts / buckets but within the same provider natively
            //}
            else
#pragma warning restore SA1513 // Closing brace should be followed by blank line
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public async Task<string> StartChunkedWriteAsync(string objectPath, CancellationToken cancellationToken = default)
        {
            var provider = _provider.GetDataStoreByObjectPath(objectPath, _serviceProvider);

            var chunkedUploadId = await provider.StartChunkedWriteAsync(objectPath, cancellationToken).ConfigureAwait(false);

            // TODO: should this have limited retry count???
            while (!ChunkedUploadsMap.TryAdd(chunkedUploadId, provider))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false);
            }

            return chunkedUploadId;
        }

        /// <inheritdoc/>
        public Task<string> WriteChunkAsync(string chunkedUploadId, Stream chunkData, CancellationToken cancellationToken = default)
        {
            // ReSharper disable once InvertIf - Makes no difference to code structure
            if (!ChunkedUploadsMap.TryGetValue(chunkedUploadId, out var provider) || provider is null)
            {
                _logger.LogError("Failed to locate provider for CancelChunkedWrite request.");
                throw new ProviderNotFoundException(
                    $"No provider was found for the chunked upload with id '{chunkedUploadId}'");
            }

            return provider.WriteChunkAsync(chunkedUploadId, chunkData, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CancelChunkedWriteAsync(string chunkedUploadId, CancellationToken cancellationToken = default)
        {
            if (!ChunkedUploadsMap.TryGetValue(chunkedUploadId, out var provider) || provider is null)
            {
                _logger.LogError("Failed to locate provider for CancelChunkedWrite request.");
                throw new ProviderNotFoundException(
                    $"No provider was found for the chunked upload with id '{chunkedUploadId}'");
            }

            await provider.CancelChunkedWriteAsync(chunkedUploadId, cancellationToken).ConfigureAwait(false);

            while (!ChunkedUploadsMap.TryUpdate(chunkedUploadId, null, provider))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false);
            }

            while (!ChunkedUploadsMap.TryRemove(chunkedUploadId, out _))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task EndChunkedWriteAsync(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails, CancellationToken cancellationToken = default)
        {
            if (!ChunkedUploadsMap.TryGetValue(chunkedUploadId, out var provider) || provider is null)
            {
                _logger.LogError("Failed to locate provider for CancelChunkedWrite request.");
                throw new ProviderNotFoundException(
                    $"No provider was found for the chunked upload with id '{chunkedUploadId}'");
            }

            await provider.EndChunkedWriteAsync(chunkedUploadId, chunkDetails, cancellationToken).ConfigureAwait(false);

            while (!ChunkedUploadsMap.TryUpdate(chunkedUploadId, null, provider))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false);
            }

            while (!ChunkedUploadsMap.TryRemove(chunkedUploadId, out _))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
