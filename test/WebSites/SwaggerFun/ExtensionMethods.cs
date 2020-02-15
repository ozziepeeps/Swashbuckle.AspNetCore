using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal static class ExtensionMethods
    {
        public static void UseFullTypeNameInSchemaIds(this SwaggerGenOptions options) => options.CustomSchemaIds(x => x.FriendlyId(true));

        private static string FriendlyId(this Type type, bool fullyQualified = false)
        {
            // This functionality exists in the .NET FX version of Swashbuckle, but not in the .NET Core version, so this function is
            // lifted from https://github.com/domaindrivendev/Swashbuckle/blob/5489aca0d2dd7946f5569341f621f581720d4634/Swashbuckle.Core/Swagger/TypeExtensions.cs.
            var typeName = fullyQualified
                ? type.FullNameSansTypeParameters().Replace("+", ".")
                : type.Name;

            if (type.IsGenericType)
            {
                var genericArgumentIds = type.GetGenericArguments()
                    .Select(t => t.FriendlyId(fullyQualified))
                    .ToArray();

                return new StringBuilder(typeName)
                    .Replace(string.Format(CultureInfo.InvariantCulture, "`{0}", genericArgumentIds.Count()), string.Empty)
                    .Append(string.Format(CultureInfo.InvariantCulture, "[{0}]", string.Join(",", genericArgumentIds).TrimEnd(',')))
                    .ToString();
            }

            return typeName;
        }

        private static string FullNameSansTypeParameters(this Type type)
        {
            // This functionality exists in the .NET FX version of Swashbuckle, but not in the .NET Core version, so this function is
            // lifted from https://github.com/domaindrivendev/Swashbuckle/blob/5489aca0d2dd7946f5569341f621f581720d4634/Swashbuckle.Core/Swagger/TypeExtensions.cs.
            var fullName = type.FullName;

            if (string.IsNullOrEmpty(fullName))
            {
                fullName = type.Name;
            }

            var chopIndex = fullName.IndexOf("[[", StringComparison.OrdinalIgnoreCase);
            return (chopIndex == -1) ? fullName : fullName.Substring(0, chopIndex);
        }
    }
}
