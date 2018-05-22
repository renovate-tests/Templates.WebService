using System;
using System.Net.Http;
using TypedRest;

namespace MyVendor.MyService
{
    public partial class Client : EntryEndpoint
    {
        // NOTE: Other parts of this class are in separate slice-specific files

        public Client(Uri uri)
            : base(uri)
        {}

        public Client(Uri uri, HttpClient httpClient)
            : base(uri, httpClient)
        {}
    }
}
