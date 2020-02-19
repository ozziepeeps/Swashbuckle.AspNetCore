using System;
using System.Runtime.Serialization;

namespace SwaggerFun.Models
{
    [Obsolete("Don't use this value!")]
    [DataContract]
    public sealed class ObsoleteType
    {
        public ObsoleteEnum Value { get; set; }
    }
}
