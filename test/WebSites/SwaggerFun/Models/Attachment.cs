using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwaggerFun.Models
{
    /// <summary>
    /// Represents an attachment.
    /// </summary>
    [DataContract]
    public sealed class Attachment
    {
        /// <summary>
        /// Gets or sets the ID of the attachment.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the caption of the attachment.
        /// </summary>
        [DataMember]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <remarks>Used to test Swagger generation.</remarks>
        [DataMember]
        public TimeSpan Expiration { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <remarks>Used to test Swagger generation.</remarks>
        [DataMember]
        public IDictionary<uint?, string> Map { get; set; }
    }
}
