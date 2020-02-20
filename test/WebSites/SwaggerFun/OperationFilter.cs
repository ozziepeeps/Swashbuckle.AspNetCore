using System;
using System.Linq;
using System.Reflection;
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
            if (!(context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                return;
            }

            ApplyOperationExtensions(operation, context, controllerActionDescriptor);
        }

        private static void ApplyOperationExtensions(OpenApiOperation operation, OperationFilterContext context, ControllerActionDescriptor controllerActionDescriptor)
        {
            var generatedCodeAttribute = controllerActionDescriptor.ControllerTypeInfo?
                .GetCustomAttributes<System.CodeDom.Compiler.GeneratedCodeAttribute>()?
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
        }
    }
}
