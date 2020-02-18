using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    public class ObjectSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (schema.Type != "object")
            {
                return;
            }

            if (KnownTypes.Types.Contains(type))
            {
                return;
            }

            if (schema.Properties == null || schema.Properties.Count == 0)
            {
                return;
            }

            var propertyLookupByAlias = new Dictionary<string, PropertyInfo>();
            var propertyLookupByPropertyName = new Dictionary<string, PropertyInfo>();
            var obsoleteLookupByAlias = new Dictionary<string, string>();
            var emitDefaultLookupByAlias = new HashSet<string>();

            foreach (var property in type.GetProperties())
            {
                var ignoreDataMemberAttribute = property.GetCustomAttribute<IgnoreDataMemberAttribute>();
                var dataMemberAttribute = property.GetCustomAttribute<DataMemberAttribute>();
                var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                var obsoleteAttribute = property.GetCustomAttribute<ObsoleteAttribute>();

                if (ignoreDataMemberAttribute != null)
                {
                    // Skip
                    continue;
                }
                else if (!string.IsNullOrWhiteSpace(dataMemberAttribute?.Name))
                {
                    // Pick up the alias from the DataMember attribute.
                    var key = dataMemberAttribute.Name.ToUpperInvariant();

                    if (!propertyLookupByAlias.ContainsKey(key))
                    {
                        propertyLookupByAlias[key] = property;
                    }

                    if (obsoleteAttribute != null)
                    {
                        obsoleteLookupByAlias[key] = obsoleteAttribute.Message ?? string.Empty;
                    }

                    if (dataMemberAttribute.EmitDefaultValue)
                    {
                        emitDefaultLookupByAlias.Add(key);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(jsonPropertyAttribute?.PropertyName))
                {
                    // Pick up the alias from the JsonProperty attribute.
                    var key = jsonPropertyAttribute.PropertyName.ToUpperInvariant();

                    if (!propertyLookupByAlias.ContainsKey(key))
                    {
                        propertyLookupByAlias[key] = property;
                    }

                    if (obsoleteAttribute != null)
                    {
                        obsoleteLookupByAlias[key] = obsoleteAttribute.Message ?? string.Empty;
                    }

                    if (jsonPropertyAttribute.DefaultValueHandling == DefaultValueHandling.Include)
                    {
                        emitDefaultLookupByAlias.Add(key);
                    }
                }
                else
                {
                    var key = property.Name.ToUpperInvariant();

                    if (!propertyLookupByAlias.ContainsKey(key))
                    {
                        propertyLookupByAlias[key] = property;
                    }

                    if (obsoleteAttribute != null)
                    {
                        obsoleteLookupByAlias[key] = obsoleteAttribute.Message ?? string.Empty;
                    }

                    if ((dataMemberAttribute?.EmitDefaultValue).GetValueOrDefault() ||
                        jsonPropertyAttribute?.DefaultValueHandling == DefaultValueHandling.Include)
                    {
                        emitDefaultLookupByAlias.Add(key);
                    }
                }

                if (!propertyLookupByPropertyName.ContainsKey(property.Name))
                {
                    propertyLookupByPropertyName[property.Name] = property;
                }
            }

            var cleanPropertyNameLookupByDirtyName = new Dictionary<string, string>();

            foreach (var property in schema.Properties)
            {
                // Models marked with [CollectionDataContract] instead of [DataContract] are susceptible to having their elements
                // serialized as "<{PropertyName}>k__BackingField" (e.g. "<Offset>k__BackingField"). We'll clean these up.
                if (property.Key.EndsWith(">k__BackingField", StringComparison.OrdinalIgnoreCase))
                {
                    var propertyName = property.Key[1..property.Key.IndexOf('>')];

                    if (propertyLookupByPropertyName.ContainsKey(propertyName))
                    {
                        var propertyInfo = propertyLookupByPropertyName[propertyName];
                        var dataMemberAttribute = propertyInfo.GetCustomAttribute<DataMemberAttribute>();
                        var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();

                        if (!string.IsNullOrWhiteSpace(dataMemberAttribute?.Name))
                        {
                            // Pick up the alias from the DataMember attribute.
                            propertyName = dataMemberAttribute.Name;
                        }
                        else if (!string.IsNullOrWhiteSpace(jsonPropertyAttribute?.PropertyName))
                        {
                            // Pick up the alias from the JsonProperty attribute.
                            propertyName = jsonPropertyAttribute.PropertyName;
                        }
                    }

                    cleanPropertyNameLookupByDirtyName.Add(property.Key, propertyName);
                }
            }

            foreach (var kvp in cleanPropertyNameLookupByDirtyName)
            {
                var item = schema.Properties[kvp.Key];
                schema.Properties.Remove(kvp.Key);
                schema.Properties.Add(kvp.Value, item);
            }

            foreach (var property in schema.Properties)
            {
                var key = property.Key.ToUpperInvariant();

                if (emitDefaultLookupByAlias.Contains(key))
                {
                    property.Value.Extensions["x-costar-serializedefault"] = new OpenApiBoolean(true);
                }

                if (propertyLookupByAlias.ContainsKey(key))
                {
                    var propertyInfo = propertyLookupByAlias[key];

                    if (!propertyInfo.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        property.Value.Extensions["x-costar-propertyname"] = new OpenApiString(propertyInfo.Name);
                    }

                    propertyInfo.PropertyType.ApplyPrimitiveExtensions(property.Value);

                    Transform(property.Value, propertyInfo.PropertyType);
                }

                if (obsoleteLookupByAlias.ContainsKey(key))
                {
                    property.Value.Extensions["x-costar-deprecated"] = new OpenApiString(obsoleteLookupByAlias[key]);
                }
            }

            var typeObsoleteAttribute = type.GetCustomAttribute<ObsoleteAttribute>();

            if (typeObsoleteAttribute != null)
            {
                schema.Extensions["x-costar-deprecated"] = new OpenApiString(typeObsoleteAttribute.Message);
            }
        }

        private static void Transform(OpenApiSchema schema, Type type)
        {
            // Ref.: https://swagger.io/docs/specification/data-models/data-types/
            // "An optional format keyword serves as a hint for the tools to use a specific numeric type."
            // Since it's just a hint, we'll can pass down a special-purpose format to help our client generation.
            type = type.UnwrapIfNullable();

            if (type == typeof(TimeSpan))
            {
                schema.Reference = null;
                schema.Type = "string";
                schema.Format = "duration";
                schema.AllOf = new List<OpenApiSchema>();   // Cannot .Clear() the collection as it's read only.
            }
            else if (type == typeof(DateTimeOffset))
            {
                schema.Format = "date-time-offset";
            }
            else if (type == typeof(IPAddress))
            {
                schema.Reference = null;
                schema.Type = "string";
                schema.Format = "ip-address";
                schema.AllOf = new List<OpenApiSchema>();   // Cannot .Clear() the collection as it's read only.
            }
            else if (type == typeof(char))
            {
                schema.Format = "char";
            }
            else if (typeof(Stream).IsAssignableFrom(type))
            {
                schema.Reference = null;
                schema.Type = "string";
                schema.Format = "stream";
                schema.Properties?.Clear();
            }
            else if (type.IsEnum)
            {
                schema.Description = type.GetEnumMembers().Describe();
            }
        }
    }
}
