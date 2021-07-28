using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>Provides a read and write builder to create and instance of the required <see cref="DataStoreProviderOptions"/>.</summary>
    [PublicAPI]
    public abstract class DataStoreProviderOptionsBuilder
    {
        private readonly Dictionary<string, object> _options;

        /// <summary>Initializes a new instance of the <see cref="DataStoreProviderOptionsBuilder"/> class.</summary>
        protected DataStoreProviderOptionsBuilder()
        {
            _options = new ();
        }

        /// <summary>Sets a named option for the derived Provider Options instance.</summary>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <param name="name">The name of the option.</param>
        /// <param name="value">The value to set for the option.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        protected void SetOption<T>([NotNull] string name, [CanBeNull] T value)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!_options.ContainsKey(name))
            {
                _options.Add(name, value);
            }
            else
            {
                _options[name] = value;
            }
        }
    }
}
