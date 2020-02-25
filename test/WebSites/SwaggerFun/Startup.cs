using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

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

                    var assembly = GetHostAssembly(services);
                    var directory = new FileInfo(assembly.Location).DirectoryName;

                    foreach (var file in Directory.GetFiles(directory, "*.xml"))
                    {
                        options.IncludeXmlComments(file);
                    }

                    options.SchemaFilter<DictionarySchemaFilter>();
                    options.SchemaFilter<EnumSchemaFilter>();
                    options.SchemaFilter<RemoveKnownTypeSchemaFilter>();
                    options.SchemaFilter<ObjectSchemaFilter>();
                    options.SchemaFilter<PrimitiveSchemaFilter>();
                    options.ParameterFilter<ParameterFilter>();
                    options.ParameterFilter<EnumParameterFilter>();
                    options.OperationFilter<OperationFilter>();
                    options.OperationFilter<OperationResponseSchemaFilter>();
                    options.DocumentFilter<DocumentFilter>();

#pragma warning disable 0618
                    // TODO (2020-01-31): Figure out an alternative to this.
                    options.DescribeAllEnumsAsStrings();
#pragma warning restore 0618
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

        private static Assembly GetHostAssembly(IServiceCollection services)
        {
            // Ref.: GetServiceFromCollection method in 
            // https://github.com/aspnet/AspNetCore/blob/708dc5cb5abd1fe0aa409f355b9e0cea8f6846d4/src/Mvc/src/Microsoft.AspNetCore.Mvc.Core/DependencyInjection/MvcCoreServiceCollectionExtensions.cs
            var hostingEnvironmentDescriptor = services.LastOrDefault(s => s.ServiceType == typeof(IHostingEnvironment));

            if (hostingEnvironmentDescriptor?.ImplementationInstance == null)
            {
                throw new InvalidOperationException($"Cannot locate implementation of {nameof(IHostingEnvironment)} in {nameof(IServiceCollection)}");
            }

            var hostingEnvironment = (IHostingEnvironment)hostingEnvironmentDescriptor.ImplementationInstance;
            var applicationName = hostingEnvironment.ApplicationName;

            // Ref.: PopulateDefaultParts method in 
            // https://github.com/aspnet/AspNetCore/blob/321327f84b2b22dcff2e9beb06a9a64236c5cced/src/Mvc/src/Microsoft.AspNetCore.Mvc.Core/ApplicationParts/ApplicationPartManager.cs
            var hostAssembly = Assembly.Load(new AssemblyName(applicationName));
            return hostAssembly;
        }
    }
}
