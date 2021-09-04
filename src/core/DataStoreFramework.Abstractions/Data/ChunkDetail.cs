using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace DataStoreFramework.Data
{
    /// <summary>Represents details to validate a data object part uploaded to the Provider's Data Store.</summary>
    [PublicAPI]
    public class ChunkDetail
    {
        /// <summary>Gets the identifier of the data object part uploaded.</summary>
        /// <remarks>This is provided by a call to <see cref="Providers.IDataStoreProvider.WriteChunkAsync(string,System.IO.Stream,System.Threading.CancellationToken)"/>.</remarks>
        [NotNull]
        [Required]
        public string Id { get; init; } = null!;

        /// <summary>Gets a collection of hash details that can be used to validate the data object part content.</summary>
        [NotNull]
        public Dictionary<HashAlgorithmName, string> Hashes { get; init; } = new ();
    }
}
