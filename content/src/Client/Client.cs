using System;
using System.Net.Http;
using MyVendor.MyService.Contacts;
using TypedRest;

namespace MyVendor.MyService
{
    /// <summary>
    /// Provides a type-safe client for the My Service REST API.
    /// </summary>
    public class Client : EntryEndpoint, IClient
    {
        /// <summary>
        /// Creates a new My Service Client.
        /// </summary>
        /// <param name="uri">The base URI of the My Service REST API.</param>
        public Client(Uri uri)
            : base(uri)
        {}

        /// <summary>
        /// Creates a new My Service Client using a custom <see cref="HttpClient"/>. This is usually used for custom authentication schemes, e.g. client certificates.
        /// </summary>
        /// <param name="uri">The base URI of the My Service REST API.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> to use for communication with My Service.</param>
        public Client(Uri uri, HttpClient httpClient)
            : base(uri, httpClient)
        {}

        /// <summary>
        /// Provides access to contacts in an address book.
        /// </summary>
        public ICollectionEndpoint<ContactDto, ContactElementEndpoint> Contacts
            => new CollectionEndpoint<ContactDto, ContactElementEndpoint>(this, relativeUri: "contacts");
    }
}
