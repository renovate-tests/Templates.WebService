using System;
using Axoom.MyService.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Axoom.MyService
{
    public abstract class ClientFactsBase : IDisposable
    {
        private readonly TestServer _server;

        protected ClientFactsBase()
        {
            _server = new TestServer(new WebHostBuilder()
                                    .ConfigureServices(x => x.AddRestApi())
                                    .ConfigureServices(ConfigureService)
                                    .Configure(x => x.UseRestApi()));
        }

        protected MyServiceClient Client => new MyServiceClient(new Uri("http://localhost"), _server.CreateClient());

        protected abstract void ConfigureService(IServiceCollection services);

        public virtual void Dispose() => _server.Dispose();
    }
}
