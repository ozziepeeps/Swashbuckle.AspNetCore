using System;
using System.Collections.Generic;

namespace SwaggerFun.Models
{
    /// <summary>
    /// Represents an attachment.
    /// </summary>
    public sealed class Attachment
    {
        /// <summary>
        /// Gets or sets the ID of the attachment.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the caption of the attachment.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <remarks>Used to test Swagger generation.</remarks>
        public TimeSpan Expiration { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <remarks>Used to test Swagger generation.</remarks>
        public IDictionary<uint?, string> Map { get; set; }
    }
}
