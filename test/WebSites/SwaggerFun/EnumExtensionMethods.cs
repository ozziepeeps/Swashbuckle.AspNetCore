using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Nexus.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal static class EnumExtensionMethods
    {
        public static string Describe(this IEnumerable<EnumMember> values)
        {
            return string.Join("<br/>", values.EmptyIfNull()
                .Select(x => string.Format(CultureInfo.CurrentCulture, "{0}&nbsp;({1})", x.Name, x.Value.ToString())))
                .TrimToNull();
        }

        public static IEnumerable<EnumMember> GetEnumMembers(this Type type)
        {
            if (type == null || !type.IsEnum)
            {
                return Enumerable.Empty<EnumMember>();
            }

            var names = type.GetEnumNames();

            var list = new List<EnumMember>();

            foreach (var name in names)
            {
                var value = Convert.ChangeType(Enum.Parse(type, name), type.GetEnumUnderlyingType(), CultureInfo.InvariantCulture);

                var field = type.GetField(name);
                var attributes = (ObsoleteAttribute[])field.GetCustomAttributes(typeof(ObsoleteAttribute), false);

                bool isObsolete;
                string obsoleteMessage;

                if (attributes != null && attributes.Any())
                {
                    isObsolete = true;
                    obsoleteMessage = attributes.First().Message;
                }
                else
                {
                    isObsolete = false;
                    obsoleteMessage = null;
                }

                list.Add(new EnumMember(name, value, isObsolete, obsoleteMessage));
            }

            return list;
        }

        public static IEnumerable<EnumMember> ApplyEnumExtensions(this Type type, IDictionary<string, IOpenApiExtension> extensions, ISchemaGenerator schemaGenerator, SchemaRepository schemaRepository)
        {
            type = type.UnwrapIfNullable();

            if (!type.IsEnum || extensions == null)
            {
                return null;
            }

            var underlyingType = type.GetEnumUnderlyingType();
            var members = type.GetEnumMembers();

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

            var enumSchema = schemaGenerator.GenerateSchema(underlyingType, schemaRepository);

            foreach (var extension in enumSchema.Extensions)
            {
                dictionary[extension.Key] = (IOpenApiAny)extension.Value;
            }

            var obsoleteAttribute = type.GetCustomAttribute<ObsoleteAttribute>();

            if (obsoleteAttribute != null)
            {
                dictionary["x-costar-deprecated"] = new OpenApiString(obsoleteAttribute.Message);
            }

            extensions[VendorExtensions.Enum] = dictionary;

            return members;
        }
    }
}
