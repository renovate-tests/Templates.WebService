using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MyVendor.MyService.Contacts
{
    public class StartupFacts : StartupFactsBase
    {
        [Fact]
        public void CanResolveContactService()
        {
            Provider.GetRequiredService<IContactService>();
        }
    }
}
