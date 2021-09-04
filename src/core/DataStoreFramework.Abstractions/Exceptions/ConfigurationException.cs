using System;
using JetBrains.Annotations;

namespace DataStoreFramework.Exceptions
{
    /// <summary>
    /// Exception for errors loading AWS options from IConfiguration object.
    /// </summary>
    [PublicAPI]
    public class ConfigurationException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ConfigurationException"/> class.</summary>
        public ConfigurationException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ConfigurationException"/> class.</summary>
        /// <param name="message">The error message.</param>
        public ConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ConfigurationException"/> class.</summary>
        /// <param name="message">The error message.</param>
        /// <param name="exception">Original exception.</param>
        public ConfigurationException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>Gets or sets the property that has an invalid value.</summary>
        public string PropertyName { get; set; }

        /// <summary>Gets or sets the value that was configured for the PropertyName.</summary>
        public string PropertyValue { get; set; }
    }
}
