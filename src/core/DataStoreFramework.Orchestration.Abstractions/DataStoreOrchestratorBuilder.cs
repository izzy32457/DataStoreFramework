using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>Stores registrations of Data Store Providers and their options to be managed by an Orchestrator.</summary>
    [PublicAPI]
    public class DataStoreOrchestratorBuilder
    {
        /// <summary>Gets or sets the Orchestrator Configuration provider.</summary>
        public IOrchestratorConfigurationProvider ConfigurationProvider { get; set; }
    }
}
