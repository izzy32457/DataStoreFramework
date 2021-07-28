using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace DataStoreFramework.Data
{
    /// <summary>Provides version metadata for a given data object.</summary>
    /// <remarks>This doesn't include details of location changes. For that information the Provider must be configured for "Auditing".</remarks>
    [PublicAPI]
    public class ObjectVersionMetadata
    {
        /// <summary>Gets the identifier for the data object version.</summary>
        [NotNull]
        [Required]
        public string VersionId { get; init; } = null!;

        /// <summary>Gets the date the data object version was created.</summary>
        [Required]
        public DateTimeOffset CreatedDate { get; init; }

        /// <summary>Gets the numerical order of the version in relation to other versions.</summary>
        [NonNegativeValue]
        public int SortOrder { get; init; }

        /// <summary>Gets a collection of hash details that can be used to validate the data object content.</summary>
        [NotNull]
        [Required]
        public Dictionary<HashAlgorithmName, string> Hashes { get; init; } = new ();
    }
}
