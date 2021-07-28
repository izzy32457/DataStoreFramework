using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace DataStoreFramework.Providers
{
    /// <summary>A base class that provides the internal component representation of a give Object Path.</summary>
    [PublicAPI]
    public abstract class DataLocator
    {
        private readonly Dictionary<string, string> _locationComponents;

        /// <summary>Initializes a new instance of the <see cref="DataLocator"/> class.</summary>
        protected DataLocator()
        {
            _locationComponents = new ();
        }

        /// <summary>Used to build an Object Path for the given derived DataLocator type.</summary>
        /// <returns>A string that represents the data object.</returns>
        [NotNull]
        public abstract string ToObjectPath();

        /// <inheritdoc/>
        public override string ToString() => ToObjectPath();

        /// <summary>Gets a named location component for the derived Data Locator instance.</summary>
        /// <param name="name">The name of the option.</param>
        /// <returns>The value of the location component specified by <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the option requested has not been set.</exception>
        [CanBeNull]
        protected string Get([CallerMemberName][NotNull] string name = null!)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _locationComponents.ContainsKey(name) ?
                _locationComponents[name] :
                throw new InvalidOperationException($"No value has be set for the location component '{name}'.");
        }

        /// <summary>Sets a named location component for the derived Data Locator instance.</summary>
        /// <param name="name">The name of the option.</param>
        /// <param name="value">The value to set for the option.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        protected void Set([CallerMemberName][NotNull] string name = null!, [CanBeNull] string value = default)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!_locationComponents.ContainsKey(name))
            {
                _locationComponents.Add(name, value);
            }
            else
            {
                _locationComponents[name] = value;
            }
        }
    }
}
