using System;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (!type.IsEnum)
            {
                return;
            }

            var underlyingType = type.GetEnumUnderlyingType();
            var enumSchema = context.SchemaGenerator.GenerateSchema(underlyingType, context.SchemaRepository);

            underlyingType.ApplyPrimitiveExtensions(enumSchema);

            var members = type.GetEnumMembers();
            schema.Description = members.Describe();

            var memberDictionary = new OpenApiObject();

            foreach (var member in members)
            {
                memberDictionary[member.Name] = new OpenApiLong(long.Parse(member.Value.ToString()));  // Will never be wider than a long.
            }

            var obsoleteDictionary = new OpenApiObject();

            foreach (var field in members.Where(f => f.IsObsolete))
            {
                obsoleteDictionary[field.Name] = new OpenApiString(field.ObsoleteMessage ?? string.Empty);
            }

            var dictionary = new OpenApiObject
            {
                ["id"] = new OpenApiString(type.FullName),
                ["fields"] = memberDictionary,
            };

            if (obsoleteDictionary.Count > 0)
            {
                dictionary["deprecated"] = obsoleteDictionary;
            }

            if (type.GetCustomAttribute<FlagsAttribute>() != null)
            {
                dictionary["flags"] = new OpenApiBoolean(true);
            }

            var obsoleteAttribute = type.GetCustomAttribute<ObsoleteAttribute>();

            if (obsoleteAttribute != null)
            {
                dictionary["x-costar-deprecated"] = new OpenApiString(obsoleteAttribute.Message);
            }

            schema.Extensions["x-costar-enum"] = dictionary;
        }
    }
}
