using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerFun
{
    public class EnumParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var type = context.ParameterInfo.ParameterType.UnwrapIfNullable();

            if (!type.IsEnum)
            {
                return;
            }

            // TODO (2020-02-14): Need this hack to work around a serialize issue with the V2 format.
            parameter.Extensions["schema"] = new OpenApiObject
            {
                ["$ref"] = new OpenApiString("#/definitions/" + type.FullName),
            };
        }
    }
}
