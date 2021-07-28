using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration.Exceptions
{
    /// <summary>Represents a generic Data Store Object Exception.</summary>
    [PublicAPI]
    public class OrchestrationException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="OrchestrationException"/> class.</summary>
        public OrchestrationException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="OrchestrationException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public OrchestrationException([CanBeNull] string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="OrchestrationException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
        public OrchestrationException([CanBeNull] string message, [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
