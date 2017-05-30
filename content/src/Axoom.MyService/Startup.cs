using System;
using System.IO;
using Axoom.Extensions.Configuration.Yaml;
using Axoom.Extensions.Logging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Axoom.MyService
{
    /// <summary>
    /// Startup class used by ASP.NET Core.
    /// </summary>
    public class Startup
    {
        [UsedImplicitly]
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Called by ASP.NET Core to set up an environment.
        /// </summary>
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true)
                .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Called by ASP.NET Core to register services.
        /// </summary>
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            AddRestApi(services);

            services
                .AddLogging()
                .AddOptions()
                //.Configure<MyOptions>(Configuration.GetSection("MyOptions"))
                //.AddSingleton<IMyRemoteService, MyRemoteService>()
                ;
        }

        /// <summary>
        /// Called by ASP.NET Core to configure services after they have been registered.
        /// </summary>
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            loggerFactory.AddAxoomLogging("Axoom.MyService");

            UseRestApi(app);
        }

        private static void AddRestApi(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Axoom.MyService",
                        Version = "v1",
                        Contact = new Contact
                        {
                            Email = "developer@axoom.com",
                            Name = "AXOOM GmbH",
                            Url = "http://developers.axoom.com"
                        }
                    });
                options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Axoom.MyService.xml"));
                options.DescribeAllEnumsAsStrings();
            });
        }

        private static void UseRestApi(IApplicationBuilder app) => app
            .UseMvc()
            .UseSwagger()
            .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Axoom.MyService API v1"));
    }
}