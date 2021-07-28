using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Exceptions
{
    /// <summary>Represents an Object Not Found Data Store Exception.</summary>
    [PublicAPI]
    public class ObjectNotFoundException : ObjectException
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectNotFoundException"/> class.</summary>
        public ObjectNotFoundException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectNotFoundException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ObjectNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectNotFoundException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
        public ObjectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
