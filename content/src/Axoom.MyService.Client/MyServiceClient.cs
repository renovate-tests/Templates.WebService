using System;
using System.Net.Http;
using Axoom.MyService.Dto;
using TypedRest;

namespace Axoom.MyService.Client
{
    public class MyServiceClient : EntryEndpoint
    {
        public MyServiceClient(Uri uri) : base(uri)
        {}

        public MyServiceClient(Uri uri, HttpClient httpClient) : base(uri, httpClient)
        {}

        public ICollectionEndpoint<Entity> Entities => new CollectionEndpoint<Entity>(this, relativeUri: "entities");
    }
}