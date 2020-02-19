using System;
using System.Runtime.Serialization;

namespace SwaggerFun.Models
{
    [DataContract]
    [Flags]
    public enum SwaggerFunEnum2 : short
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        One = 1,

        [EnumMember]
        [Obsolete("Don't use this value!")]
        Two = 2
    }
}
