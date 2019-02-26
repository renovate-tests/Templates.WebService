using System;
using Xunit.Abstractions;

namespace MyVendor.MyService
{
    /// <summary>
    /// Sets up an in-memory version of the ASP.NET MVC stack for decoupled testing of controllers and the <see cref="Client"/> library.
    /// </summary>
    public abstract class ApiFactsBase : ControllerFactsBase
    {
        protected ApiFactsBase(ITestOutputHelper output)
            : base(output)
        {
            Client = new Client(new Uri("http://localhost"), HttpHandler);
        }

        /// <summary>
        /// A client configured for in-memory communication with ASP.NET MVC controllers.
        /// </summary>
        protected readonly Client Client;
    }
}
