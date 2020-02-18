using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class KnownTypeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (type == typeof(IPAddress))
            {
                schema.Extensions["x-costar-remove"] = new OpenApiBoolean(true);
            }

            if (type == typeof(AddressFamily))
            {
                schema.Extensions["x-costar-remove"] = new OpenApiBoolean(true);
            }

            if (type == typeof(TimeSpan))
            {
                schema.Extensions["x-costar-remove"] = new OpenApiBoolean(true);
            }
        }
    }
}
