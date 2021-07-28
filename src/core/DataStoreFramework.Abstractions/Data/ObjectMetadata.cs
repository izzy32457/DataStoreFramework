using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using JetBrains.Annotations;

namespace DataStoreFramework.Data
{
    [PublicAPI]
    public class ObjectMetadata
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
        public DateTimeOffset ModifiedDate { get; set; }

        public ICollection<ObjectVersionMetadata> Versions { get; set; }
    }
}
