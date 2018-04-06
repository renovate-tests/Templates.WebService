using Axoom.MyService.Contacts;

// ReSharper disable once CheckNamespace
namespace Axoom.MyService
{
    public partial class MyServiceClient
    {
        public ContactCollectionEndpoint Contacts => new ContactCollectionEndpoint(this);
    }
}