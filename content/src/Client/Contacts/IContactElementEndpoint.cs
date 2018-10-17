using TypedRest;

namespace MyVendor.MyService.Contacts
{
    public interface IContactElementEndpoint : IElementEndpoint<ContactDto>
    {
        /// <summary>
        /// An optional note on the contact.
        /// </summary>
        IElementEndpoint<NoteDto> Note { get; }

        /// <summary>
        /// A action for poking the contact.
        /// </summary>
        IActionEndpoint Poke { get; }
    }
}
