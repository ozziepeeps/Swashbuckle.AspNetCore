using System;

namespace SwaggerFun.Models
{
    public enum SwaggerFunEnum : short
    {
        Unknown = 0,

        One = 1,

        [Obsolete("Don't use this value!")]
        Two = 2
    }
}
