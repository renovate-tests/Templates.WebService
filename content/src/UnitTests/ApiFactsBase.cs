using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace MyVendor.MyService
{
    /// <summary>
    /// Sets up an in-memory version of the ASP.NET MVC stack for decoupled testing of controllers and the <see cref="Client"/> library.
    /// </summary>
    public abstract class ApiFactsBase : ControllerFactsBase
    {
        protected ApiFactsBase(ITestOutputHelper output, IDictionary<string, string> configuration = null)
            : base(output, configuration)
        {}

        /// <summary>
        /// A client configured for in-memory communication with ASP.NET MVC controllers.
        /// </summary>
        protected Client Client => new Client(new Uri("http://localhost"), HttpClient);
    }
}
