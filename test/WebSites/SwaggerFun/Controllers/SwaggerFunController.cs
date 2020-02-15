using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwaggerFun.Models;

namespace SwaggerFun.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SwaggerFunController
    {
        [HttpGet("{id}")]
        public async Task<SwaggerFunModel> GetSwaggerFunAsync(int id, SwaggerFunEnum? type, int query, CancellationToken cancellationToken)
        {
            return new SwaggerFunModel();
        }

        [HttpGet("swaggerfun/enum")]
        public async Task<SwaggerFunEnum> GetSwaggerFunEnumAsync(CancellationToken cancellationToken)
        {
            return SwaggerFunEnum.One;
        }
    }
}
