using System;
using System.Collections.Generic;
using System.Text;

namespace VivesRental.RestApi.Contracts.V1.Responses
{
    public class PagedResponse<T>
    {
        public PagedResponse()
        {

        }

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
    }
}
