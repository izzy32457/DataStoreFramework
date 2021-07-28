using System;
using DataStoreFramework.Exceptions;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration.Exceptions
{
    /// <summary>Represents a Provider Not Found Data Store Exception.</summary>
    [PublicAPI]
    public class ProviderNotFoundException : OrchestrationException
    {
        /// <summary>Initializes a new instance of the <see cref="ProviderNotFoundException"/> class.</summary>
        public ProviderNotFoundException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ProviderNotFoundException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ProviderNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ProviderNotFoundException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
        public ProviderNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
