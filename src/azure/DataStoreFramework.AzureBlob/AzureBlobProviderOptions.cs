using System.Collections.Generic;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.AzureBlob
{
    /// <summary>AWS S3 specific implementation of <see cref="ProviderOptions"/>.</summary>
    [PublicAPI]
    public class AzureBlobProviderOptions : ProviderOptions
    {
        /// <summary>Initializes a new instance of the <see cref="AzureBlobProviderOptions"/> class.</summary>
        /// <param name="options">An already defined set of options for initialization.</param>
        internal AzureBlobProviderOptions(Dictionary<string, object> options)
            : base(options)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AzureBlobProviderOptions"/> class.</summary>
        internal AzureBlobProviderOptions()
        {
        }

        /// <summary>Gets the connection string for the blob store.</summary>
        public string ConnectionString
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        /// <summary>Gets the name of the configured blob container.</summary>
        public string ContainerName
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }
    }
}
