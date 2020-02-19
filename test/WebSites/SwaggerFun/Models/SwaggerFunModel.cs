using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace SwaggerFun.Models
{
    /// <summary>
    /// Represents a DTO that is fun to represent in Swagger.
    /// </summary>
    [DataContract]
    public class SwaggerFunModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public uint? Id { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        [DataMember]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        [DataMember]
        public IDictionary<uint?, string> Map { get; set; }

        /// <summary>
        /// Gets or sets the second map.
        /// </summary>
        [DataMember]
        public IDictionary<Guid, int?> Map2 { get; set; }

        /// <summary>
        /// Gets or sets the third map.
        /// </summary>
        [DataMember]
        public IDictionary<Guid, Attachment> Map3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth map.
        /// </summary>
        [DataMember]
        public IDictionary<SwaggerFunEnum, Attachment> Map4 { get; set; }

        /// <summary>
        /// Gets or sets the fifth map.
        /// </summary>
        [DataMember]
        public IReadOnlyDictionary<SwaggerFunEnum, Attachment> Map5 { get; set; }

        /// <summary>
        /// Gets or sets the sixth map.
        /// </summary>
        [DataMember]
        public IReadOnlyDictionary<string, SwaggerFunEnum2> Map6 { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [DataMember]
        public SwaggerFunEnum Enumeration { get; set; }

        /// <summary>
        /// Gets or sets the nullable type.
        /// </summary>
        [DataMember]
        public SwaggerFunEnum? NullableEnumeration { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        [DataMember]
        public SwaggerFunEnum[] EnumerationArray { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public IEnumerable<SwaggerFunEnum> EnumerationEnumerable { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public IReadOnlyList<SwaggerFunEnum> EnumerationReadOnlyList { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        [DataMember]
        public string[] StringArray { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public IEnumerable<string> StringEnumerable { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public IReadOnlyList<string> StringReadOnlyList { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        [DataMember]
        public string[][] StringJaggedArray { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        /// <remarks>TODO (2019-07-30): This is not correctly represented.</remarks>
        [DataMember]
        public string[,] StringSquareArray { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public IEnumerable<string[]> StringArrayEnumerable { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public IReadOnlyList<string[]> StringArrayReadOnlyList { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public KeyValuePair<string, int> KeyValuePair { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        [DataMember]
        public KeyValuePair<string, IEnumerable<string>> NestedGenerics { get; set; }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        /// <remarks>Represented in Swagger as a string with a special format.</remarks>
        [DataMember]
        public IPAddress Address { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <remarks>Represented in Swagger as a string with a special format.</remarks>
        [DataMember]
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets a type.
        /// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
        [DataMember]
        public ObsoleteType ObsoleteType { get; set; }
#pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        /// Gets or sets an obsolete property.
        /// </summary>
        [DataMember]
        [Obsolete("Obsolete property")]
        public bool ObsoleteProperty { get; set; }
    }
}
