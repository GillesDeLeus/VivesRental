using VivesRental.RestApi.Contracts.V1.Responses;
using VivesRental.Services.Model;

namespace VivesRental.RestApi.Extensions
{
    public static class PaginationExtensions
    {
        public static PagedResponse<T> CreatePagedResponse<T>(this PagedServiceResult<T> serviceResult,
            PagingCriteria pagingCriteria)
        {
            //var nextPage = pagingCriteria.PageNumber >= 1
            //    ? uriService.GetAllPostsUri(new PagingQuery(pagingCriteria.PageNumber + 1,
            //        pagingCriteria.PageSize)) : null;
            //var previousPage = pagingCriteria.PageNumber - 1 >= 1
            //    ? uriService.GetAllPostsUri(new PagingQuery(pagingCriteria.PageNumber - 1,
            //        pagingCriteria.PageSize)) : null;

            var pagedResponse = new PagedResponse<T>
            {
                //Data = serviceResult.Data,
                PageNumber = pagingCriteria.PageNumber >= 1 ? pagingCriteria.PageNumber : (int?) null,
                PageSize = pagingCriteria.PageSize >= 1 ? pagingCriteria.PageSize : (int?) null,
                //NextPage = data.Any() ? nextPage : null,
                //PreviousPage = previousPage
            };

            return pagedResponse;
        }
    }
}
