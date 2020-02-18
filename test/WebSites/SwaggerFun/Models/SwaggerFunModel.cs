using System;
using System.Collections.Generic;

namespace SwaggerFun.Models
{
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
    }
}
