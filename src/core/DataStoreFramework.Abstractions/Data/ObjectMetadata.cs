using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using JetBrains.Annotations;

namespace DataStoreFramework.Data
{
    /// <summary>Provides metadata for a data object and it's versions.</summary>
    [PublicAPI]
    public class ObjectMetadata
    {
        /// <summary>Gets the data object location that can be used by a Data Store Provider.</summary>
        [NotNull]
        [Required]
        public string ObjectPath { get; init; } = null!;

        /// <summary>Gets or sets the path within the Data Store that the file is located.</summary>
        /// <remarks>This can be obfuscated if the provider is configured for "SecureStorage".</remarks>
        [CanBeNull]
        public string Key { get; set; }

        /// <summary>Gets the MIME Type of the data object.</summary>
        /// <remarks>The default value used is 'application/octet-stream'.</remarks>
        [NotNull]
        [Required]
        [DefaultValue(MediaTypeNames.Application.Octet)]
        public string MimeType { get; init; } = MediaTypeNames.Application.Octet;

        /// <summary>Gets the date the data object was created.</summary>
        /// <remarks>For some providers this is the date the first version was uploaded.</remarks>
        [Required]
        public DateTimeOffset CreatedDate =>
            Versions.FirstOrDefault(v => v.SortOrder == Versions.Min(x => x.SortOrder))?.CreatedDate
            ?? DateTimeOffset.MinValue;

        /// <summary>Gets the date the data object was last modified.</summary>
        /// <remarks>For some providers this is the date the latest version was uploaded.</remarks>
        [Required]
        public DateTimeOffset ModifiedDate =>
            Versions.FirstOrDefault(v => v.SortOrder == Versions.Max(x => x.SortOrder))?.CreatedDate
            ?? DateTimeOffset.MaxValue;

        /// <summary>Gets a collection of version details for this data object.</summary>
        /// <remarks>This will have a maximum of 2 versions if the Provider is not configured for "Versioning".</remarks>
        [NotNull]
        public ICollection<ObjectVersionMetadata> Versions { get; init; } = new List<ObjectVersionMetadata>();
    }
}
