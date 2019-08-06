using MyVendor.MyService.Contacts;
using TypedRest;

namespace MyVendor.MyService
{
    public interface IClient : IEndpoint
    {
        /// <summary>
        /// Provides access to contacts in an address book.
        /// </summary>
        ICollectionEndpoint<Contact, ContactElementEndpoint> Contacts { get; }
    }
}