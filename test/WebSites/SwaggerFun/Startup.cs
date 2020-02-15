using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SwaggerFun
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSwaggerGen(options => 
                {
                    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Test API", Version = "V1.0.1" });

                    options.UseFullTypeNameInSchemaIds();

                    options.SchemaFilter<SchemaFilter>();
                    options.SchemaFilter<EnumSchemaFilter>();
                    options.ParameterFilter<ParameterFilter>();
                    options.ParameterFilter<EnumParameterFilter>();
                    options.OperationFilter<OperationFilter>();
#pragma warning disable 0618
                    // TODO (2020-01-31): Figure out an alternative to this.
                    options.DescribeAllEnumsAsStrings();
#pragma warning restore 0618
                    options.DocumentFilter<DocumentFilter>();
                })
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwagger(x => x.SerializeAsV2 = true)
                .UseSwaggerUI(x => x.SwaggerEndpoint("v1/swagger.json", "V1.0.1"))
                .UseRouting()
                .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
