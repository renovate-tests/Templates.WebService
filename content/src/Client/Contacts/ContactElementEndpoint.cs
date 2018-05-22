using System;
using JetBrains.Annotations;
using TypedRest;

namespace MyVendor.MyService.Contacts
{
    [UsedImplicitly]
    public class ContactElementEndpoint : ElementEndpoint<ContactDto>
    {
        public ContactElementEndpoint(IEndpoint referrer, Uri relativeUri)
            : base(referrer, relativeUri.EnsureTrailingSlash())
        {}

        public ElementEndpoint<NoteDto> Note => new ElementEndpoint<NoteDto>(this, relativeUri: "note");

        public ActionEndpoint Poke => new ActionEndpoint(this, relativeUri: "poke");
    }
}
