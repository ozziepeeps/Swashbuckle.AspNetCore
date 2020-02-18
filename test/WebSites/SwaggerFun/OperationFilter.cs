using System;
using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class OperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            var generatedCodeAttribute = controllerActionDescriptor.ControllerTypeInfo?
                .GetCustomAttributes(true).OfType<GeneratedCodeAttribute>()?
                .FirstOrDefault(a => a.Tool == "costar.swagger.DomainId/costar.swagger.OperationId");

            if (generatedCodeAttribute != null)
            {
                var metadata = generatedCodeAttribute.Version.Split('/');

                if (metadata.Length == 2)
                {
                    operation.Extensions["x-costar-domain-id"] = new OpenApiString(metadata[0]);
                    operation.Extensions["x-costar-operation-id"] = new OpenApiString(metadata[1]);
                }
            }

            var methodId = context.MethodInfo.Name.Replace("Async", string.Empty);
            operation.Extensions["x-costar-method-id"] = new OpenApiString(methodId);

            var obsoleteMessages = context.MethodInfo
                .GetCustomAttributes(true)
                .Union(controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true))
                .OfType<ObsoleteAttribute>()
                .Select(attr => attr.Message)
                .Distinct();

            if (obsoleteMessages.Any())
            {
                operation.Extensions["x-costar-deprecated"] = new OpenApiString(obsoleteMessages.First());
            }

            foreach (var response in operation.Responses)
            {
                foreach (var content in response.Value.Content)
                {
                    var returnType = context.MethodInfo.ReturnType.UnwrapIfTask();
                    returnType.ApplyTypeModifierExtensions(content.Value);
                }
            }
        }
    }
}
