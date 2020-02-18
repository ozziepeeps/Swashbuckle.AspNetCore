using System;

namespace SwaggerFun.Models
{
    [Flags]
    public enum SwaggerFunEnum2 : short
    {
        Unknown = 0,

        One = 1,

        [Obsolete("Don't use this value!")]
        Two = 2
    }
}
