using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Polly;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Axoom.MySolutionName
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup.
        /// </summary>
        /// <param name="env">The hosting environment.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services collection to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                });

            services
                .AddLogging()
                .AddOptions()
                //.Configure<MyOptions>(Configuration.GetSection("MyOptions"))
                ;

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "MyServiceName",
                        Version = "v1",
                        Contact = new Contact
                        {
                            Email = "developer@axoom.com",
                            Name = "AXOOM GmbH",
                            Url = "http://developers.axoom.com"
                        }
                    });
                options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Axoom.MySolutionName.xml"));
                options.DescribeAllEnumsAsStrings();
            });
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="provider">The service provider.</param>
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AXOOM DataHub - IoT API v1");
            });

            var startupLogger = loggerFactory.CreateLogger<Startup>();
            var policy = Policy
                .Handle<SocketException>()
                .WaitAndRetry(
                    sleepDurations: new[] {TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20)},
                    onRetry: (ex, timeSpan) => startupLogger.LogWarning("Problem connecting to external service. Retrying in {0}.", timeSpan));

            //policy.Execute(provider.GetService<MyService>);
        }
    }
}
