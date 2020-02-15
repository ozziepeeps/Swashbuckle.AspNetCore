using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Nexus.Extensions;

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

        public static void ApplyEnumExtensions(this Type type, IOpenApiExtensible extensible, IEnumerable<EnumMember> members)
        {
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

            extensible.Extensions["x-costar-enum"] = dictionary;
        }
    }
}
