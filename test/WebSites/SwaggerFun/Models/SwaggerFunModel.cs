using System;
using System.Collections.Generic;

namespace SwaggerFun.Models
{
    /// <summary>
    /// Represents a DTO that is fun to represent in Swagger.
    /// </summary>
    public class SwaggerFunModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public uint? Id { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        public IDictionary<uint?, string> Map { get; set; }

        /// <summary>
        /// Gets or sets the second map.
        /// </summary>
        public IDictionary<Guid, int?> Map2 { get; set; }

        /// <summary>
        /// Gets or sets the third map.
        /// </summary>
        public IDictionary<Guid, Attachment> Map3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth map.
        /// </summary>
        public IDictionary<SwaggerFunEnum, Attachment> Map4 { get; set; }

        /// <summary>
        /// Gets or sets the fifth map.
        /// </summary>
        public IReadOnlyDictionary<SwaggerFunEnum, Attachment> Map5 { get; set; }

        /// <summary>
        /// Gets or sets the sixth map.
        /// </summary>
        public IReadOnlyDictionary<string, SwaggerFunEnum2> Map6 { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public SwaggerFunEnum Enumeration { get; set; }

        /// <summary>
        /// Gets or sets the nullable type.
        /// </summary>
        public SwaggerFunEnum? NullableEnumeration { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        public SwaggerFunEnum[] EnumerationArray { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        public IEnumerable<SwaggerFunEnum> EnumerationEnumerable { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        public IReadOnlyList<SwaggerFunEnum> EnumerationReadOnlyList { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        public string[] StringArray { get; set; }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        public IEnumerable<string> StringEnumerable { get; set; }


        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        public IReadOnlyList<string> StringReadOnlyList { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        public string[][] StringJaggedArray { get; set; }

        /// <summary>
        /// Gets or sets the type array.
        /// </summary>
        /// <remarks>TODO (2019-07-30): This is not correctly represented.</remarks>
        public string[,] StringSquareArray { get; set; }
    }
}
