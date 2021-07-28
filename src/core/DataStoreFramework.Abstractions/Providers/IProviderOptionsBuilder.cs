using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>Provides a read and write builder to create and instance of the required <see cref="ProviderOptions"/>.</summary>
    [PublicAPI]
    public interface IProviderOptionsBuilder
    {
        /// <summary>Sets a named option for the derived Provider Options instance.</summary>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <param name="name">The name of the option.</param>
        /// <param name="value">The value to set for the option.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        void SetOption<T>([NotNull] string name, [CanBeNull] T value);
    }

    /// <summary>Provides a read and write builder to create and instance of the required <see cref="ProviderOptions"/>.</summary>
    /// <typeparam name="TProviderOptions">The type of the required <see cref="ProviderOptions"/>.</typeparam>
    [PublicAPI]
    public interface IProviderOptionsBuilder<out TProviderOptions> : IProviderOptionsBuilder
        where TProviderOptions : ProviderOptions
    {
        /// <summary>Builds an instance of the required <typeparamref name="TProviderOptions"/>.</summary>
        /// <returns>An instance of <typeparamref name="TProviderOptions"/> with all configured options set.</returns>
        TProviderOptions Build();
    }
}
