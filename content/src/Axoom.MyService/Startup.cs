using System;
using System.IO;
using System.Net.Sockets;
using Axoom.Extensions.Configuration.FileExtensions;
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
using Polly;
using Swashbuckle.AspNetCore.Swagger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Axoom.MyService
{
    /// <summary>
    /// Startup.
    /// </summary>
    [UsedImplicitly]
    public class Startup
    {
        private const string APPLICATION_NAME = "Axoom.MyService";

        /// <summary>
        /// Startup.
        /// </summary>
        /// <param name="env">The hosting environment.</param>
        public Startup(IHostingEnvironment env)
        {
            Configuration = CreateConfiguration();
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

            services.AddSingleton(CreateLoggerFactory());
            services.AddOptions()
                //.Configure<MyOptions>(Configuration.GetSection("MyOptions"))
                ;

            ConfigureSwaggerService(services);
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
            loggerFactory.AddAxoomLogging("Axoom.MyService");

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Axoom.MyService API v1"));

            var startupLogger = loggerFactory.CreateLogger<Startup>();
            var policy = Policy
                .Handle<SocketException>()
                .WaitAndRetry(
                    sleepDurations: new[] {TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20)},
                    onRetry: (ex, timeSpan) => startupLogger.LogWarning("Problem connecting to external service. Retrying in {0}.", timeSpan));

            //policy.Execute(provider.GetService<MyService>);
        }

        private static void ConfigureSwaggerService(IServiceCollection services)
        {
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
                options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Axoom.MyService.xml"));
                options.DescribeAllEnumsAsStrings();
            });
        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            ILoggerFactory loggerFactory = new LoggerFactory().WithFilter(new FilterLoggerSettings
            {
                {"Microsoft", LogLevel.None}
            });
            return loggerFactory;
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .UseAxoomConfiguration(applicationName: APPLICATION_NAME.ToLowerInvariant())
                .AddYamlFiles()
                .AddEnvironmentVariables(prefix: $"{APPLICATION_NAME.ToLowerInvariant()}:");
            return builder.Build();
        }
    }
}