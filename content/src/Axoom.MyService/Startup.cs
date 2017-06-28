using System;
using Axoom.Extensions.Configuration.Yaml;
using Axoom.Extensions.Logging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services
                .AddRestApi()
                .AddLogging()
                .AddOptions()
                //.Configure<MyOptions>(Configuration.GetSection("MyOptions"))
                //.AddTransient<IMyService, MyService>()
                //.AddSingleton<Worker>()
                ;
        }

        /// <summary>
        /// Called by ASP.NET Core to configure services after they have been registered.
        /// </summary>
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            app.UseRestApi();

            loggerFactory.AddAxoomLogging("Axoom.MyService");

            //var logger = loggerFactory.CreateLogger<Startup>();
            //var policy = Policy
            //    .Handle<SocketException>()
            //    .Or<HttpRequestException>()
            //    .WaitAndRetryAsync(
            //        sleepDurations: new[] { TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20) },
            //        onRetry: (ex, timeSpan) => logger.LogWarning(0, ex,
            //            "Problem connecting to external service. Retrying in {0}.", timeSpan));

            //policy.ExecuteAsync(async () =>
            //{
            //    await provider.GetService<Worker>().StartAsync();
            //}).Wait();
        }
    }
}