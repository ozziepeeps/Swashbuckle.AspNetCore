using System;
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
        [HttpGet("{id}/{type}")]
        public async Task<SwaggerFunModel> GetSwaggerFunAsync(int id, SwaggerFunEnum? type, int query, CancellationToken cancellationToken)
        {
            return new SwaggerFunModel();
        }

        [HttpGet("enum")]
        public async Task<SwaggerFunEnum> GetSwaggerFunEnumAsync(CancellationToken cancellationToken)
        {
            return SwaggerFunEnum.One;
        }

        [HttpGet("enum/array")]
        public async Task<SwaggerFunEnum[]> GetSwaggerFunEnumArrayAsync(CancellationToken cancellationToken)
        {
            return Array.Empty<SwaggerFunEnum>();
        }

        [HttpGet("int/array")]
        public async Task<int[]> GetSwaggerFunIntArrayAsync(CancellationToken cancellationToken)
        {
            return Array.Empty<int>();
        }

        [HttpGet("nullablebool")]
        public async Task<bool?> GetSwaggerFunNullableBoolAsync(CancellationToken cancellationToken)
        {
            return true;
        }

        [HttpGet("obsolete")]
        [Obsolete("Use something else!")]
        public async Task GetSwaggerFunObsoleteAsync(CancellationToken cancellationToken)
        {
        }
    }
}
