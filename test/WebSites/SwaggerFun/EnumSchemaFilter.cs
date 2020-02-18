using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    public class EnumSchemaFilter : ISchemaFilter
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

            underlyingType.ApplyTypeModifierExtensions(enumSchema);

            var members = type.GetEnumMembers();
            schema.Description = members.Describe();

            type.ApplyEnumExtensions(schema, members);
        }
    }
}
