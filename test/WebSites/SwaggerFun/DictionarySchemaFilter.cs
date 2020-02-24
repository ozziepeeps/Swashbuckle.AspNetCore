using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class DictionarySchemaFilter : ISchemaFilter
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

            var valueType = dictionaryType.GetGenericArguments()[1];
            valueType.ApplyPrimitiveExtensions(schema.AdditionalProperties?.Extensions);
            valueType.ApplyEnumExtensions(schema.AdditionalProperties?.Extensions, context.SchemaGenerator, context.SchemaRepository);

            var dictionary = new OpenApiObject();

            if (keySchema.Reference != null)
            {
                dictionary["$ref"] = new OpenApiString(keySchema.Reference.ReferenceV2);
            }
            else
            {
                dictionary["type"] = new OpenApiString(keySchema.Type);

                if (keySchema.Format != null)
                {
                    dictionary["format"] = new OpenApiString(keySchema.Format);
                }
            }

            foreach (var extension in keySchema.Extensions)
            {
                dictionary[extension.Key] = (IOpenApiAny)extension.Value;
            }

            schema.Extensions[VendorExtensions.KeySchema] = dictionary;

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
