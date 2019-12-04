using System.Collections.Generic;
using System.Linq;

namespace VivesRental.Services.Model
{
    public class PagedServiceResult<T>
    {
        public PagedServiceResult(T result)
        {
            Data = result;
        }

        public T Data { get; set; }

        public IEnumerable<ServiceError> Errors { get; set; }

        public bool IsSuccess => Errors == null || !Errors.Any();
        public PagingCriteria PagingCriteria { get; set; }
    }
}
