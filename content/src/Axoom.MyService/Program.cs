using Microsoft.AspNetCore.Hosting;

namespace Axoom.MyService
{
    public static class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args) => new WebHostBuilder()
            .UseUrls("http://*:80", "http://*:5000") // 80 for API, 5000 for metrics
            .UseKestrel()
            .UseStartup<Startup>()
            .Build();
    }
}
