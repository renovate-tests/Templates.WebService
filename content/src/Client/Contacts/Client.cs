using MyVendor.MyService.Contacts;

// ReSharper disable once CheckNamespace
namespace MyVendor.MyService
{
    public partial class Client
    {
        /// <summary>
        /// Provides access to contacts in an address book.
        /// </summary>
        public ContactCollectionEndpoint Contacts => new ContactCollectionEndpoint(this);
    }
}
