using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SwaggerFun
{
    internal static class KnownTypes
    {
        public static readonly Type[] Types = new[]
        {
            typeof(AddressFamily),
            typeof(IPAddress),
            typeof(Stream),
            typeof(TimeSpan),
        };
    }
}
