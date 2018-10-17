using System;
using JetBrains.Annotations;
using TypedRest;

namespace MyVendor.MyService.Contacts
{
    /// <summary>
    /// Represents a REST endpoint for a single <see cref="ContactDto"/>.
    /// </summary>
    [UsedImplicitly]
    public class ContactElementEndpoint : ElementEndpoint<ContactDto>, IContactElementEndpoint
    {
        public ContactElementEndpoint(IEndpoint referrer, Uri relativeUri)
            : base(referrer, relativeUri.EnsureTrailingSlash())
        {}

        /// <summary>
        /// An optional note on the contact.
        /// </summary>
        public IElementEndpoint<NoteDto> Note => new ElementEndpoint<NoteDto>(this, relativeUri: "note");

        /// <summary>
        /// A action for poking the contact.
        /// </summary>
        public IActionEndpoint Poke => new ActionEndpoint(this, relativeUri: "poke");
    }
}
