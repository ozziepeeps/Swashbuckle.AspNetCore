using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            var members = type.ApplyEnumExtensions(schema.Extensions, context.SchemaGenerator, context.SchemaRepository);

            if (members != null)
            {
                schema.Description = members.Describe();
            }
        }
    }
}
