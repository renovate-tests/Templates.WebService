using System;
using System.Net.Http;
using TypedRest;

namespace MyVendor.MyService
{
    /// <summary>
    /// Provides a type-safe client for the My Service REST API.
    /// </summary>
    public partial class Client : EntryEndpoint
    {
        // NOTE: Other parts of this class are in separate slice-specific files

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
    }
}
