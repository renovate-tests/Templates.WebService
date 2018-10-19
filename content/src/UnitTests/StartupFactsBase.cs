using System;
using Microsoft.Extensions.DependencyInjection;

namespace MyVendor.MyService
{
    public abstract class StartupFactsBase
    {
        protected readonly IServiceProvider Provider;

        protected StartupFactsBase()
        {
            var services = new ServiceCollection();
            new Startup().ConfigureServices(services);
            Provider = services.BuildServiceProvider();
        }
    }
}
