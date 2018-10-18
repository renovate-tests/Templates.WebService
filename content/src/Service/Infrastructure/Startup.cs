using System;
using Axoom.Extensions.Logging.Console;
using Axoom.Extensions.Prometheus.Standalone;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyVendor.MyService.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services.AddSingleton(configuration)
                       .AddOptions()
                       .AddLogging(builder => builder.AddConfiguration(configuration.GetSection("Logging"))
                                                     .AddExceptionDemystifyer())
                       .AddPrometheusServer(configuration)
                       .AddPolicies(configuration)
                       .AddRestApi();

        public static IServiceProvider UseInfrastructure(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;

            provider.GetRequiredService<ILoggerFactory>()
                    .AddAxoomConsole(provider.GetRequiredService<IConfiguration>().GetSection("Logging"))
                    .CreateLogger("Startup")
                    .LogInformation("Starting My Service");

            provider.UsePrometheusServer();

            app.UseRestApi();

            return provider;
        }
    }
}
