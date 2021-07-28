using JetBrains.Annotations;

namespace DataStoreFramework.Data
{
    /// <summary>Possible framework serialization types.</summary>
    [PublicAPI]
    [NoReorder]
    public enum SerializationType
    {
        /// <summary>This is used to determine no serialization required.</summary>
        /// <remarks>When used this will likely throw an exception.</remarks>
        Raw = 0,

        /// <summary>This is used to specify JSON serialization.</summary>
        /// <remarks>When used the framework must already be configured to use the required JSON serializer library.</remarks>
        Json = 1,

        /// <summary>This is used to specify XML serialization.</summary>
        Xml = 2,
    }
}
