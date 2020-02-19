using System;
using System.Runtime.Serialization;

namespace SwaggerFun.Models
{
    [DataContract]
    [Flags]
    [Obsolete]
    public enum ObsoleteEnum
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        One = 1,

        [EnumMember]
        Two = 2
    }
}
