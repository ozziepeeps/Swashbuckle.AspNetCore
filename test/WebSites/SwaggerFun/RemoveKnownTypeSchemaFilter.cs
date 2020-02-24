using System;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class RemoveKnownTypeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (KnownTypes.Types.Contains(type))
            {
                // Just annotate for now. The schemas are actually removed from the document by the DocumentFilter.
                schema.Extensions.Add(DocumentFilter.Remove, new OpenApiBoolean(true));
            }
        }
    }
}
