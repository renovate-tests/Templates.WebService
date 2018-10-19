using Microsoft.AspNetCore.Hosting;

namespace MyVendor.MyService
{
    public static class Program
    {
        public static void Main()
            => new WebHostBuilder()
              .UseKestrel()
              .UseStartup<Startup>()
              .Build()
              .Run();
    }
}
