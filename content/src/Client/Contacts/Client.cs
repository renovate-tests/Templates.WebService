using MyVendor.MyService.Contacts;

// ReSharper disable once CheckNamespace
namespace MyVendor.MyService
{
    public partial class Client
    {
        public ContactCollectionEndpoint Contacts => new ContactCollectionEndpoint(this);
    }
}
