using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    public class DictionarySchemaFilter : SchemaFilterBase, ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            var genericTypes = type.GetGenericTypes();

            var dictionaryType =
                genericTypes.FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ??
                genericTypes.FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));

            if (dictionaryType == null)
            {
                return;
            }

            var keyType = dictionaryType.GetGenericArguments()[0];
            var keySchema = context.SchemaGenerator.GenerateSchema(keyType, context.SchemaRepository);

            keyType.ApplyTypeModifierExtensions(keySchema);

            var dictionary = new OpenApiObject
            {
                ["type"] = new OpenApiString(keySchema.Type),
            };

            if (keySchema.Format != null)
            {
                dictionary["format"] = new OpenApiString(keySchema.Format);
            }

            foreach (var extension in keySchema.Extensions)
            {
                dictionary[extension.Key] = (IOpenApiAny)extension.Value;
            }

            schema.Extensions["x-costar-keyschema"] = dictionary;

            if (schema != null)
            {
                if (schema.Properties != null)
                {
                    var first = schema.Properties.Values.FirstOrDefault();

                    if (first != null)
                    {
                        // Re-write the schema to be the same "shape" as other dictionaries.
                        schema.Properties = null;
                        schema.AdditionalProperties = first;
                    }
                }
            }
        }
    }
}
