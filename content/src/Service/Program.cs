using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyVendor.MyService
{
    public static class Program
    {
        public static void Main()
            => new WebHostBuilder()
              .UseKestrel(x => x.Listen(IPAddress.Any, x.ApplicationServices.GetRequiredService<IConfiguration>().GetValue<int>("Port")))
              .UseStartup<Startup>()
              .Build()
              .Run();
    }
}
