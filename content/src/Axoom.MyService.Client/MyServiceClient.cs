using System;
using System.Net.Http;
using Axoom.MyService.Dto;
using JetBrains.Annotations;
using TypedRest;

namespace Axoom.MyService.Client
{
    public class MyServiceClient : EntryEndpoint
    {
        public MyServiceClient(Uri uri) : base(uri)
        {}

        public MyServiceClient(Uri uri, HttpClient httpClient) : base(uri, httpClient)
        {}

        public ContactCollectionEndpoint Contacts => new ContactCollectionEndpoint(this);
    }

    public class ContactCollectionEndpoint : CollectionEndpoint<ContactDto, ContactElementEndpoint>
    {
        public ContactCollectionEndpoint(IEndpoint referrer) : base(referrer, relativeUri: "contacts")
        {}
    }

    [UsedImplicitly]
    public class ContactElementEndpoint : ElementEndpoint<ContactDto>
    {
        public ContactElementEndpoint(IEndpoint referrer, Uri relativeUri) : base(referrer, relativeUri.EnsureTrailingSlash())
        {}

        public ElementEndpoint<NoteDto> Note => new ElementEndpoint<NoteDto>(this, relativeUri: "note");

        public ActionEndpoint Poke => new ActionEndpoint(this, relativeUri: "poke");
    }
}