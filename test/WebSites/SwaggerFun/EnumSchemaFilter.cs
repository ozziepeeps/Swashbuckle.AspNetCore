using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
            {
                return;
            }

            var type = context.Type;

            var underlyingType = context.Type.GetEnumUnderlyingType();
            var enumSchema = context.SchemaGenerator.GenerateSchema(underlyingType, context.SchemaRepository);

            underlyingType.ApplyPrimitiveExtensions(enumSchema);

            var members = type.GetEnumMembers();
            schema.Description = members.Describe();

            type.ApplyEnumExtensions(schema, members);
        }
    }
}
