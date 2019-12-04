using System;
using VivesRental.RestApi.Services.Contracts;

namespace VivesRental.RestApi.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri BuildGetUri(string uri, Guid id)
        {
            return new Uri(_baseUri + uri.Replace("{id}", id.ToString()));
        }

    }
}
