using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyVendor.MyService
{
    /// <summary>
    /// Provides an entry point to the API.
    /// </summary>
    [ApiController, Route("")]
    [AllowAnonymous]
    public class RootController : Controller
    {
        [HttpGet("")]
        public string Bla() => "My Service API";
    }
}
