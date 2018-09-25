using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MyVendor.MyService.Infrastructure;
using Xunit.Abstractions;

namespace MyVendor.MyService
{
    public abstract class ClientFactsBase : IDisposable
    {
        private readonly TestServer _server;

        protected ClientFactsBase(ITestOutputHelper output)
        {
            _server = new TestServer(new WebHostBuilder()
                                    .ConfigureServices(x => x.AddLogging(builder => builder.AddXunit(output))
                                                             .AddRestApi())
                                    .ConfigureServices(ConfigureService)
                                    .Configure(x => x.UseRestApi()));
        }

        protected Client Client => new Client(new Uri("http://localhost"), _server.CreateClient());

        protected abstract void ConfigureService(IServiceCollection services);

        public virtual void Dispose() => _server.Dispose();
    }
}
