using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3
{
    /// <summary>A ProviderOptions builder for Amazon S3 storage provider implementation.</summary>
    [PublicAPI]
    public class AwsS3ProviderOptionsBuilder : ProviderOptionsBuilder, IProviderOptionsBuilder<AwsS3ProviderOptions>
    {
        /// <inheritdoc/>
        public AwsS3ProviderOptions Build()
            => new (Options);
    }
}
