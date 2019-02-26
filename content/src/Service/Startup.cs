using System;
using Axoom.Extensions.Prometheus.Standalone;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyVendor.MyService.Contacts;
using MyVendor.MyService.Infrastructure;

namespace MyVendor.MyService
{
    /// <summary>
    /// Configures dependency injection.
    /// </summary>
    [UsedImplicitly]
    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Called by ASP.NET Core to set up an environment.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Called by ASP.NET Core to register services.
        /// </summary>
        public IServiceProvider ConfigureServices(IServiceCollection services)
            => services.AddOptions()
                       .AddDbContext<DbContext>(options => options.UseSqlite(Configuration.GetSection("Database").GetValue<string>("ConnectionString")))
                       .AddPrometheusServer(Configuration.GetSection("Metrics"))
                       .AddPolicies(Configuration.GetSection("Policies"))
                       .AddSecurity(Configuration.GetSection("Authentication"))
                       .AddRestApi()
                       .AddContacts()
                       .BuildServiceProvider();

        /// <summary>
        /// Called by ASP.NET Core to configure services after they have been registered.
        /// </summary>
        public void Configure(IApplicationBuilder app)
        {
            app.UseSecurity()
               .UseRestApi();

            var provider = app.ApplicationServices;

            // Since SQLite is an in-process database resiliency against connectivity problems at startup is unnecessary.
            // It is implemented here anyway as a sample in case you decide to use an external database such as PostgreSQL.
            provider.GetRequiredService<Policies>().Startup(() =>
            {
                using (var scope = provider.CreateScope())
                    // Replace .EnsureCreated() with .Migrate() once you have generated an EF Migration
                    scope.ServiceProvider.GetRequiredService<DbContext>().Database.EnsureCreated();
            });
        }
    }
}
