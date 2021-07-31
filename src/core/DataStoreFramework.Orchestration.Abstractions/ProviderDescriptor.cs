using System;
using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>Describes a provider with its implementation type, configuration options, and lifetime.</summary>
    [PublicAPI]
    public sealed class ProviderDescriptor
    {
        /// <summary>Gets the type of the provider.</summary>
        public Type ProviderType { get; init; }

        /// <summary>Gets the options to create a provider instance.</summary>
        public ProviderOptions Options { get; init; }

        /// <summary>Gets the Identifier of the provider.</summary>
        public string Identifier => Options.Identifier ??
                                    ProviderType.AssemblyQualifiedName ??
                                    ProviderType.FullName ??
                                    ProviderType.GUID.ToString("D");

        /// <summary>Creates an instance of <see cref="ProviderDescriptor" /> with the specified serviceType, implementationFactory, and lifetime.</summary>
        /// <param name="providerType">The type of the provider.</param>
        /// <param name="options">The options to create a provider instance.</param>
        /// <returns>A new instance of <see cref="ProviderDescriptor"/>.</returns>
        public static ProviderDescriptor Describe(Type providerType, ProviderOptions options)
        {
            return new ()
            {
                ProviderType = providerType,
                Options = options,
            };
        }

        /// <summary>Creates an instance of <see cref="ProviderDescriptor" /> with the specified serviceType, implementationFactory, and lifetime.</summary>
        /// <typeparam name="TProvider">The type of the provider.</typeparam>
        /// <typeparam name="TOptions">The type of the provider options.</typeparam>
        /// <param name="options">The options to create a provider instance.</param>
        /// <returns>A new instance of <see cref="ProviderDescriptor"/>.</returns>
        public static ProviderDescriptor Describe<TProvider, TOptions>(TOptions options)
            where TOptions : ProviderOptions
            => Describe(typeof(TProvider), options);
    }
}
