using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace DataStoreFramework.Data
{
    [PublicAPI]
    public class ObjectVersionMetadata
    {
        [Key]
        [Required]
        public string Key { get; set; }

        [Required]
        [DefaultValue(MediaTypeNames.Application.Octet)]
        public string MimeType { get; set; } = MediaTypeNames.Application.Octet;

        [Required]
        public DateTimeOffset CreatedDate { get; set; }

        [Required]
        public string VersionId { get; set; }

        public int SortOrder { get; set; }

        [Required]
        public Dictionary<HashAlgorithmName, string> Hashes { get; set; }
    }
}
