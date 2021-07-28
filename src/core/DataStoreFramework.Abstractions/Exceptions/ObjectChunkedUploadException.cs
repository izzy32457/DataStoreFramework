using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Exceptions
{
    /// <summary>Represents an Object Not Found Data Store Exception.</summary>
    [PublicAPI]
    public class ObjectChunkedUploadException : ObjectException
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectChunkedUploadException"/> class.</summary>
        public ObjectChunkedUploadException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectChunkedUploadException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ObjectChunkedUploadException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectChunkedUploadException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
        public ObjectChunkedUploadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
