using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        public void Copy(string sourceObjectPath, string destinationObjectPath)
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

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - TODO: need to test if this is actually the case. It should not be true.
            if (ReferenceEquals(srcProvider, destinationObjectPath))
            {
                srcProvider.Copy(sourceObjectPath, destinationObjectPath);
            }

            if (srcProvider.Type == destProvider.Type)
            {
                // TODO: How could we implement copy across accounts / buckets but within the same provider natively
            }

            using var stream = srcProvider.Read(sourceObjectPath);
            destProvider.Write(destinationObjectPath, stream);
        }

        /// <inheritdoc/>
        public void Move(string sourceObjectPath, string destinationObjectPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string StartChunkedWrite(string objectPath)
        {
            var provider = _provider.GetDataStoreByObjectPath(objectPath, _serviceProvider);

            var chunkedUploadId = provider.StartChunkedWrite(objectPath);

            while (!ChunkedUploadsMap.TryAdd(chunkedUploadId, provider))
            {
                Thread.Sleep(10);
            }

            return chunkedUploadId;
        }

        /// <inheritdoc/>
        public string WriteChunk(string chunkedUploadId, Stream chunkData)
        {
            // ReSharper disable once InvertIf - Makes no difference to code structure
            if (!ChunkedUploadsMap.TryGetValue(chunkedUploadId, out var provider) || provider is null)
            {
                _logger.LogError("Failed to locate provider for CancelChunkedWrite request.");
                throw new ProviderNotFoundException(
                    $"No provider was found for the chunked upload with id '{chunkedUploadId}'");
            }

            return provider.WriteChunk(chunkedUploadId, chunkData);
        }

        /// <inheritdoc/>
        public void CancelChunkedWrite(string chunkedUploadId)
        {
            if (!ChunkedUploadsMap.TryGetValue(chunkedUploadId, out var provider) || provider is null)
            {
                _logger.LogError("Failed to locate provider for CancelChunkedWrite request.");
                throw new ProviderNotFoundException(
                    $"No provider was found for the chunked upload with id '{chunkedUploadId}'");
            }

            provider.CancelChunkedWrite(chunkedUploadId);

            while (!ChunkedUploadsMap.TryUpdate(chunkedUploadId, null, provider))
            {
                Thread.Sleep(10);
            }

            while (!ChunkedUploadsMap.TryRemove(chunkedUploadId, out _))
            {
                Thread.Sleep(10);
            }
        }

        /// <inheritdoc/>
        public void EndChunkedWrite(string chunkedUploadId, [InstantHandle] IEnumerable<ChunkDetail> chunkDetails)
        {
            if (!ChunkedUploadsMap.TryGetValue(chunkedUploadId, out var provider) || provider is null)
            {
                _logger.LogError("Failed to locate provider for CancelChunkedWrite request.");
                throw new ProviderNotFoundException(
                    $"No provider was found for the chunked upload with id '{chunkedUploadId}'");
            }

            provider.EndChunkedWrite(chunkedUploadId, chunkDetails);

            while (!ChunkedUploadsMap.TryUpdate(chunkedUploadId, null, provider))
            {
                Thread.Sleep(10);
            }

            while (!ChunkedUploadsMap.TryRemove(chunkedUploadId, out _))
            {
                Thread.Sleep(10);
            }
        }
    }
}
