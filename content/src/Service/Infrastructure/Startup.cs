using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Axoom.MyService.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services.AddSingleton(configuration)
                       .AddOptions()
                       .AddAxoomLogging(configuration)
                       .AddPolicies(configuration.GetSection("Policies"))
                       .AddMetrics()
                       .AddRestApi();

        public static IServiceProvider UseInfrastructure(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;

            provider.UseAxoomLogging();
            provider.ExposeMetrics(port: 5000);

            app.UseRestApi();

            return provider;
        }
    }
}
