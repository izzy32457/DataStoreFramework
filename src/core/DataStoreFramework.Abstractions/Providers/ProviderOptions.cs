using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>A base class that defines the common Data Store Provider configuration options.</summary>
    /// <remarks>This should be inherited by each provider specific implementation.</remarks>
    [PublicAPI]
    public abstract class ProviderOptions
    {
        private readonly Dictionary<string, object> _options;

        /// <summary>Initializes a new instance of the <see cref="ProviderOptions"/> class.</summary>
        /// <param name="options">An already defined set of options for initialization.</param>
        protected ProviderOptions([NotNull] Dictionary<string, object> options)
        {
            _options = options;
        }

        /// <summary>Initializes a new instance of the <see cref="ProviderOptions"/> class.</summary>
        protected ProviderOptions()
        {
            _options = new ();
        }

        /// <summary>Gets the Data Store Provider identifier.</summary>
        /// <remarks>This is used when orchestration multiple Data Stores.</remarks>
        [CanBeNull]
        public string Identifier
        {
            get => GetOption<string>();
            init => SetOption(value: value);
        }

        /// <summary>Gets the maximum size for a data object part.</summary>
        /// <remarks>This is used by Data Store clients to select the preferred upload method.</remarks>
        public ushort MaxDataPartSize
        {
            get => GetOption<ushort>();
            init => SetOption(value: value);
        }

        /// <summary>Gets a named option for the derived Provider Options instance.</summary>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <param name="name">The name of the option.</param>
        /// <returns>The value of the option cast to the specified <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the option requested has not been set.</exception>
        protected T GetOption<T>([CallerMemberName][NotNull] string name = null!)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _options.ContainsKey(name) ?
                (T)_options[name] :
                throw new InvalidOperationException($"No value has be set for the option '{name}'.");
        }

        /// <summary>Sets a named option for the derived Provider Options instance.</summary>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <param name="name">The name of the option.</param>
        /// <param name="value">The value to set for the option.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        protected void SetOption<T>([CallerMemberName][NotNull] string name = null!, T value = default)
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
