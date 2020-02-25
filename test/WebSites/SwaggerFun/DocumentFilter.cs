using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal sealed class DocumentFilter : IDocumentFilter
    {
        public const string Remove = "remove"; // Not actually a vendor extension since it never gets emitted into the JSON.

        public void Apply(OpenApiDocument document, DocumentFilterContext context)
        {
            RemoveModels(document);

            //RollUpEnums(document);
        }

        private static void RemoveModels(OpenApiDocument document)
        {
            var modelIdsToRemove = new List<string>();

            foreach (var model in document.Components.Schemas)
            {
                if (model.Value.Extensions.ContainsKey(DocumentFilter.Remove))
                {
                    modelIdsToRemove.Add(model.Key);
                }
            }

            foreach (var modelId in modelIdsToRemove)
            {
                document.Components.Schemas.Remove(modelId);
            }
        }

        //private static void RollUpEnums(OpenApiDocument document)
        //{
        //    var enums = new Dictionary<string, object>();

        //    foreach (var path in document.Paths.Values)
        //    {
        //        var operations = new[] { path.Delete, path.Get, path.Head, path.Options, path.Patch, path.Post, path.Put }.Where(o => o != null);

        //        foreach (var operation in operations)
        //        {
        //            foreach (var parameter in operation.Parameters.EmptyIfNull())
        //            {
        //                GatherEnums(parameter.Extensions, enums);
        //            }

        //            foreach (var response in operation.Responses.EmptyIfNull())
        //            {
        //                if (response.Value.Schema != null)
        //                {
        //                    GatherEnums(response.Value.Schema.Extensions, enums);

        //                    if (response.Value.Schema.Items != null)
        //                    {
        //                        GatherEnums(response.Value.Schema.Items.Extensions, enums);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    foreach (var model in document.Definitions.Values)
        //    {
        //        foreach (var property in model.Properties.EmptyIfNull())
        //        {
        //            GatherEnums(property.Value.Extensions, enums);

        //            if (property.Value.AdditionalProperties?.Extensions != null)
        //            {
        //                GatherEnums(property.Value.AdditionalProperties.Extensions, enums);
        //            }

        //            if (property.Value.Items != null)
        //            {
        //                GatherEnums(property.Value.Items.Extensions, enums);
        //            }
        //        }
        //    }

        //    swaggerDoc.Extensions[VendorExtensions.Enums] = enums;
        //}

        //private static void GatherEnums(Dictionary<string, object> extensions, Dictionary<string, object> enums)
        //{
        //    if (extensions != null)
        //    {
        //        if (extensions.ContainsKey(VendorExtensions.KeySchema))
        //        {
        //            // Keyschemas can themselves be enums.
        //            GatherEnums((Dictionary<string, object>)extensions[VendorExtensions.KeySchema], enums);
        //        }

        //        if (extensions.ContainsKey(VendorExtensions.Enum))
        //        {
        //            var enumSchema = (Dictionary<string, object>)extensions[VendorExtensions.Enum];
        //            var enumId = enumSchema["id"].ToString();
        //            enumSchema.Remove("id");

        //            if (!enums.ContainsKey(enumId))
        //            {
        //                enums[enumId] = enumSchema;
        //            }

        //            extensions.Remove(VendorExtensions.Enum);

        //            var reference = new Dictionary<string, object>
        //            {
        //                ["$ref"] = $"#/{VendorExtensions.Enums}/" + enumId
        //            };

        //            extensions[VendorExtensions.Enum] = reference;
        //        }
        //    }
        //}
    }
}
