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
    }
}
