using System;
using System.Linq;
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

            if (KnownTypes.Types.Contains(type))
            {
                schema.Extensions["x-costar-remove"] = new OpenApiBoolean(true);
            }
        }
    }
}
