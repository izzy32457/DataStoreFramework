using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>A collection of extensions to simplify configuring <see cref="ProviderOptions"/>.</summary>
    [PublicAPI]
    public static class ProviderOptionsBuilderExtensions
    {
        /// <summary>Sets the name / identifier for the Data Store Provider instance.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Provider Options Builder</typeparam>
        /// <param name="builder">The Provider Options Builder instance to configure.</param>
        /// <param name="name">The name to use for the Data Store Provider instance.</param>
        /// <returns>The <paramref name="builder"/> instance with the Identifier set.</returns>
        /// <remarks>This is only used when orchestrating multiple data stores.</remarks>
        public static TOptionsBuilder WithName<TOptionsBuilder>(this TOptionsBuilder builder, string name)
            where TOptionsBuilder : IProviderOptionsBuilder
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetOption(nameof(ProviderOptions.Identifier), name);
            return builder;
        }

        /// <summary>Sets the maximum Data Object upload size before requiring chunked upload process.</summary>
        /// <typeparam name="TOptionsBuilder">The specific type of the Provider Options Builder</typeparam>
        /// <param name="builder">The Provider Options Builder instance to configure.</param>
        /// <param name="maximumPartSize">The maximum part size.</param>
        /// <returns>The <paramref name="builder"/> instance with the MaxDataPartSize set.</returns>
        public static TOptionsBuilder SetMaxFilePartSize<TOptionsBuilder>(this TOptionsBuilder builder, ushort maximumPartSize)
            where TOptionsBuilder : IProviderOptionsBuilder
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetOption(nameof(ProviderOptions.MaxDataPartSize), maximumPartSize);
            return builder;
        }
    }
}
