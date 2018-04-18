using TypedRest;

namespace Axoom.MyService.Contacts
{
    public class ContactCollectionEndpoint : CollectionEndpoint<ContactDto, ContactElementEndpoint>
    {
        public ContactCollectionEndpoint(IEndpoint referrer)
            : base(referrer, relativeUri: "contacts")
        {}
    }
}
