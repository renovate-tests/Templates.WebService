using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MyVendor.MyService
{
    public static class Program
    {
        public static void Main()
            => new WebHostBuilder().UseKestrel()
                                   .ConfigureAppConfiguration(Configuration)
                                   .UseStartup<Startup>()
                                   .Build()
                                   .Run();

        private static void Configuration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            var env = context.HostingEnvironment;
            builder.SetBasePath(env.ContentRootPath)
                  .AddYamlFile("appsettings.yml", optional: false, reloadOnChange: true)
                  .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables();
        }
    }
}
