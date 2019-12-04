using System;

namespace VivesRental.RestApi.Services.Contracts
{
    /// <summary>
    /// Generates Uris for the Api
    /// </summary>
    public interface IUriService
    {
        Uri BuildGetUri(string uri, Guid id);
    }
}
