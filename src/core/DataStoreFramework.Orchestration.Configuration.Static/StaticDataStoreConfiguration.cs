using System.Collections.Generic;

namespace DataStoreFramework.Orchestration
{
    /// <summary>A model type for retrieving Data Store configuration from appSettings.</summary>
    public class StaticDataStoreConfiguration
    {
        /// <summary>Gets or sets the string name for the Data Store Provider type (fully qualified assembly name).</summary>
        public string FullyQualifiedTypeName { get; set; }

        /// <summary>Gets or sets the list of options for the specified Data Store Provider.</summary>
        public Dictionary<string, object> Options { get; set; }
    }
}
