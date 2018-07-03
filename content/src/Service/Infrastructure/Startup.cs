using System;
using Axoom.Extensions.Prometheus.Standalone;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyVendor.MyService.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services.AddSingleton(configuration)
                       .AddOptions()
                       .AddAxoomLogging(configuration)
                       .AddPrometheusServer(configuration)
                       .AddPolicies(configuration.GetSection("Policies"))
                       .AddRestApi();

        public static IServiceProvider UseInfrastructure(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;

            provider.UseAxoomLogging();
            provider.UsePrometheusServer();

            app.UseRestApi();

            return provider;
        }
    }
}
