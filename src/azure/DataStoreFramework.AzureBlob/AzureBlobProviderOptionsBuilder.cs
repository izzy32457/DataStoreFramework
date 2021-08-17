using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.AzureBlob
{
    /// <summary>A ProviderOptions builder for Azure Blob storage provider implementation.</summary>
    [PublicAPI]
    public class AzureBlobProviderOptionsBuilder : ProviderOptionsBuilder, IProviderOptionsBuilder<AzureBlobProviderOptions>
    {
        /// <inheritdoc/>
        public AzureBlobProviderOptions Build()
            => new (Options);
    }
}
