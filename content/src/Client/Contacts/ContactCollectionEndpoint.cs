using TypedRest;

namespace MyVendor.MyService.Contacts
{
    /// <summary>
    /// Represents a REST endpoint for a collection of <see cref="ContactDto"/>s.
    /// </summary>
    public class ContactCollectionEndpoint : CollectionEndpoint<ContactDto, ContactElementEndpoint>
    {
        public ContactCollectionEndpoint(IEndpoint referrer)
            : base(referrer, relativeUri: "contacts")
        {}
    }
}
