using System.Collections.Generic;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3
{
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

        public string Region
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        public string ApplicationKey
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        public string SecretKey
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        public string Username
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        public string Password
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        public string Profile
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        public bool EnforcePathStyle
        {
            get => GetOption<bool>();
            init => SetOption(value: value);
        }
    }
}
