using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>Provides a read and write builder to create and instance of the required <see cref="ProviderOptions"/>.</summary>
    [PublicAPI]
    public class ProviderOptionsBuilder : IProviderOptionsBuilder
    {
        /// <summary>Store of all options that have been configured.</summary>
#pragma warning disable CA1051 // Do not declare visible instance fields
#pragma warning disable SA1401 // Fields should be private
        protected readonly Dictionary<string, object> Options;
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore CA1051 // Do not declare visible instance fields

        /// <summary>Initializes a new instance of the <see cref="ProviderOptionsBuilder"/> class.</summary>
        public ProviderOptionsBuilder()
        {
            Options = new ();
        }

        /// <summary>Sets a named option for the derived Provider Options instance.</summary>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <param name="name">The name of the option.</param>
        /// <param name="value">The value to set for the option.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        public void SetOption<T>(string name, T value)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!Options.ContainsKey(name))
            {
                Options.Add(name, value);
            }
            else
            {
                Options[name] = value;
            }
        }
    }
}
