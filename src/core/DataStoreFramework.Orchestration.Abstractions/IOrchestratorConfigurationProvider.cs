using DataStoreFramework.Providers;
using JetBrains.Annotations;

namespace DataStoreFramework.Orchestration
{
    /// <summary>Defines the requirements for an Orchestrator Configuration provider.</summary>
    [PublicAPI]
    public interface IOrchestratorConfigurationProvider
    {
        /// <summary>Retrieves an instance of a registered provider by it's registered identifier.</summary>
        /// <param name="name">The name of the required provider.</param>
        /// <returns>An instance of the specified Data Store Provider.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found with the specified <paramref name="name"/>.</exception>
        [NotNull]
        IDataStoreProvider GetDataStoreByName([NotNull] string name);

        /// <summary>Retrieves an instance of a registered provider by it's registered identifier.</summary>
        /// <param name="objectPath">The path for an object stored in the required provider.</param>
        /// <returns>An instance of the specified Data Store Provider.</returns>
        /// <exception cref="Exceptions.ProviderNotFoundException">Thrown if no provider is found that supports the specified <paramref name="objectPath"/>.</exception>
        [NotNull]
        IDataStoreProvider GetDataStoreByObjectPath([NotNull] string objectPath);
    }
}
