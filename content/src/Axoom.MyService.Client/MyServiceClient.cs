using System;
using System.Net.Http;
using TypedRest;

namespace Axoom.MyService
{
    public partial class MyServiceClient : EntryEndpoint
    {
        // NOTE: Other parts of this class are in separate slice-specific files

        public MyServiceClient(Uri uri) : base(uri)
        {}

        public MyServiceClient(Uri uri, HttpClient httpClient) : base(uri, httpClient)
        {}
    }
}