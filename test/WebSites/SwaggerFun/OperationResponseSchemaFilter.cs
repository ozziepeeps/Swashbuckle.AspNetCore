using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class OperationResponseSchemaFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var response in operation.Responses)
            {
                foreach (var content in response.Value.Content)
                {
                    //var returnType = context.MethodInfo.ReturnType.UnwrapIfTask();
                    //returnType.ApplyPrimitiveExtensions(content.Value.Schema);
                }
            }
        }
    }
}
