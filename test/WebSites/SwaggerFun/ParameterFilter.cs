using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    internal class ParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var type = context.ParameterInfo.ParameterType;
            type.ApplyPrimitiveExtensions(parameter.Extensions);

            var members = type.ApplyEnumExtensions(parameter.Extensions, context.SchemaGenerator, context.SchemaRepository);

            if (members != null)
            {
                parameter.Description = members.Describe();
            }
        }
    }
}
