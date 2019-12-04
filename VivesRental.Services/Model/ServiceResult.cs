using System.Collections.Generic;
using System.Linq;

namespace VivesRental.Services.Model
{
    public class ServiceResult<T>
    {
        public ServiceResult(T result)
        {
            Data = result;
        }

        public T Data { get; set; }

        public IList<ServiceError> Errors { get; set; }

        public bool IsSuccess => Errors == null || !Errors.Any();
    }
}
