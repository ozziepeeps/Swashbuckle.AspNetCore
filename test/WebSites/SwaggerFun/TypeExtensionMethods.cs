using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;

namespace SwaggerFun
{
    internal static class TypeExtensionMethods
    {
        public static void ApplyPrimitiveExtensions(this Type type, IDictionary<string, IOpenApiExtension> extensions)
        {
            if (type == null || extensions == null)
            {
                return;
            }

            int? byteSize = null;
            var unsigned = false;

            if (type.IsNullable(out var nullableTypeArgument))
            {
                type = nullableTypeArgument;
                extensions[VendorExtensions.Nullable] = new OpenApiBoolean(true);
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
                extensions[VendorExtensions.ByteSize] = new OpenApiInteger(byteSize.Value);
            }

            if (unsigned)
            {
                extensions[VendorExtensions.Unsigned] = new OpenApiBoolean(true);
            }
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