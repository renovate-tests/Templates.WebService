using System;
using System.Net.Http;
using MyVendor.MyService.Contacts;
using TypedRest;

namespace MyVendor.MyService
{
    /// <summary>
    /// Provides a type-safe client for the My Service REST API.
    /// </summary>
    public class Client : OAuthEntryEndpoint, IClient
    {
        /// <summary>
        /// Creates a new My Service Client.
        /// </summary>
        /// <param name="uri">The base URI of the My Service API.</param>
        /// <param name="oAuthOptions">Options for OAuth 2.0 / OpenID Connect authentication.</param>
        public Client(Uri uri, OAuthOptions oAuthOptions)
            : base(uri, oAuthOptions)
        {}

        /// <summary>
        /// Creates a new My Service Client using a custom <see cref="HttpMessageHandler"/>. This is usually used for custom authentication schemes (e.g. client certificates) or testing.
        /// </summary>
        /// <param name="uri">The base URI of the My Service API.</param>
        /// <param name="httpMessageHandler">The HTTP message handler used to communicate with the remote element.</param>
        public Client(Uri uri, HttpMessageHandler httpMessageHandler)
            : base(uri, oAuthOptions: null, httpMessageHandler)
        {}

        /// <summary>
        /// Provides access to contacts in an address book.
        /// </summary>
        public ICollectionEndpoint<ContactDto, ContactElementEndpoint> Contacts
            => new CollectionEndpoint<ContactDto, ContactElementEndpoint>(this, relativeUri: "contacts");
    }
}
