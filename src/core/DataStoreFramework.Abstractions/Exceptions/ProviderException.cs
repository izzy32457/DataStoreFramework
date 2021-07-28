using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Exceptions
{
    /// <summary>Represents a generic Data Store Provider Exception.</summary>
    [PublicAPI]
    public class ProviderException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ProviderException"/> class.</summary>
        public ProviderException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ProviderException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ProviderException([CanBeNull] string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ProviderException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
        public ProviderException([CanBeNull] string message, [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
