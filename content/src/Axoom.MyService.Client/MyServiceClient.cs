using System;
using System.Net.Http;
using Axoom.MyService.Contacts;
using TypedRest;

namespace Axoom.MyService
{
    public class MyServiceClient : EntryEndpoint
    {
        public MyServiceClient(Uri uri) : base(uri)
        {}

        public MyServiceClient(Uri uri, HttpClient httpClient) : base(uri, httpClient)
        {}

        public ContactCollectionEndpoint Contacts => new ContactCollectionEndpoint(this);
    }
}