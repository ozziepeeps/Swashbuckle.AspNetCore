using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;

namespace SwaggerFun
{
    internal static class TypeExtensionMethods
    {
        public static void ApplyPrimitiveExtensions(this Type type, IOpenApiExtensible extensible)
        {
            if (type == null || extensible == null)
            {
                return;
            }

            int? byteSize = null;
            var unsigned = false;

            if (type.IsNullable(out var nullableTypeArgument))
            {
                type = nullableTypeArgument;
                extensible.Extensions["x-costar-nullable"] = new OpenApiBoolean(true);
            }

            type = type.UnwrapIfArray().UnwrapIfNullable();

            switch (type.FullName)
            {
                case "System.Byte":
                    byteSize = 1;
                    unsigned = true;
                    break;

                case "System.SByte":
                    byteSize = 1;
                    break;

                case "System.Int16":
                    byteSize = 2;
                    break;

                case "System.UInt16":
                    byteSize = 2;
                    unsigned = true;
                    break;

                case "System.Int32":
                case "System.Single":
                    byteSize = 4;
                    break;

                case "System.UInt32":
                    byteSize = 4;
                    unsigned = true;
                    break;

                case "System.Int64":
                case "System.Double":
                    byteSize = 8;
                    break;

                case "System.UInt64":
                    byteSize = 8;
                    unsigned = true;
                    break;

                case "System.Decimal":
                    // Not technically a primitive, but often used like it's one.
                    byteSize = 16;
                    break;
            }

            if (byteSize != null)
            {
                extensible.Extensions["x-costar-bytesize"] = new OpenApiInteger(byteSize.Value);
            }

            if (unsigned)
            {
                extensible.Extensions["x-costar-unsigned"] = new OpenApiBoolean(true);
            }
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

        public static Type UnwrapIfNullable(this Type type)
        {
            if (type.IsNullable(out var nullableTypeArgument))
            {
                return nullableTypeArgument;
            }

            return type;
        }

        public static Type UnwrapIfArray(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            return type;
        }

        public static Type UnwrapIfTask(this Type type)
        {
            if (type.IsTask(out var taskTypeArgument))
            {
                return taskTypeArgument;
            }

            return type;
        }

        public static bool IsNullable(this Type type, out Type nullableTypeArgument)
        {
            nullableTypeArgument = null;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                nullableTypeArgument = type.GetGenericArguments().Single();
                return true;
            }

            return false;
        }

        public static bool IsTask(this Type type, out Type taskTypeArgument)
        {
            taskTypeArgument = null;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                taskTypeArgument = type.GetGenericArguments().Single();
                return true;
            }

            return false;
        }

        public static IEnumerable<Type> GetGenericTypes(this Type type)
        {
            var genericTypes = type.GetInterfaces().Where(t => t.IsGenericType).ToList();

            if (type.IsGenericType)
            {
                genericTypes.Add(type);
            }

            return genericTypes;
        }
    }
}
