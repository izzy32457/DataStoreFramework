using System.Collections.Generic;
using Amazon;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3
{
    /// <summary>AWS S3 specific implementation of <see cref="ProviderOptions"/>.</summary>
    [PublicAPI]
    public class AwsS3ProviderOptions : ProviderOptions
    {
        /// <summary>Initializes a new instance of the <see cref="AwsS3ProviderOptions"/> class.</summary>
        /// <param name="options">An already defined set of options for initialization.</param>
        internal AwsS3ProviderOptions(Dictionary<string, object> options)
            : base(options)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AwsS3ProviderOptions"/> class.</summary>
        internal AwsS3ProviderOptions()
        {
        }

        /// <summary>Gets the configured <see cref="RegionEndpoint"/>.</summary>
        public RegionEndpoint Region
        {
            get => GetOption<RegionEndpoint>();
            init => SetOption(value: value);
        }

        /// <summary>Gets the access key for service credentials.</summary>
        public string AccessKey
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        /// <summary>Gets the secret key for service credentials.</summary>
        public string SecretKey
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        /// <summary>Gets the profile name to use for service credentials.</summary>
        public string Profile
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        /// <summary>Gets a value indicating whether Path Style should be forced.</summary>
        public bool ForcePathStyle
        {
            get => GetOption<bool>();
            init => SetOption(value: value);
        }

        /// <summary>Gets a custom service endpoint URL.</summary>
        public string ServiceEndpoint
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }
    }
}
