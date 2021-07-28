using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Exceptions
{
    /// <summary>Represents a generic Data Store Object Exception.</summary>
    [PublicAPI]
    public class ObjectException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectException"/> class.</summary>
        public ObjectException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ObjectException([CanBeNull] string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
        public ObjectException([CanBeNull] string message, [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
