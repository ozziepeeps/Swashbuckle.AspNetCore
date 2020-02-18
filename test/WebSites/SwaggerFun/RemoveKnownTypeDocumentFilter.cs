using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class RemoveKnownTypeDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var modelIdsToRemove = new List<string>();

            foreach (var model in swaggerDoc.Components.Schemas)
            {
                if (model.Value.Extensions.ContainsKey("x-costar-remove"))
                {
                    modelIdsToRemove.Add(model.Key);
                }
            }

            foreach (var modelId in modelIdsToRemove)
            {
                swaggerDoc.Components.Schemas.Remove(modelId);
            }
        }
    }
}
