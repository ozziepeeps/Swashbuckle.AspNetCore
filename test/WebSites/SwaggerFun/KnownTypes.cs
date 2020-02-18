using System;
using System.Net;
using System.Net.Sockets;

namespace SwaggerFun
{
    public static class KnownTypes
    {
        public static readonly Type[] Types = new[]
        {
            typeof(IPAddress),
            typeof(AddressFamily),
            typeof(TimeSpan),
        };
    }
}
