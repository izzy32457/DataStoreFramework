using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>Provides a read and write builder to create and instance of the required <see cref="DataStoreProviderOptions"/>.</summary>
    /// <typeparam name="TProviderOptions">The type of the required <see cref="DataStoreProviderOptions"/>.</typeparam>
    [PublicAPI]
    public interface IDataStoreProviderOptionsBuilder<out TProviderOptions>
        where TProviderOptions : DataStoreProviderOptions
    {
        /// <summary>Builds an instance of the required <typeparamref name="TProviderOptions"/>.</summary>
        /// <returns>An instance of <typeparamref name="TProviderOptions"/> with all configured options set.</returns>
        TProviderOptions Build();
    }
}
