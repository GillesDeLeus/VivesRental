using System.Linq;
using VivesRental.Services.Model;

namespace VivesRental.Services.Extensions
{
    public static class PagingExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingCriteria criteria)
        {
            if (criteria == null)
            {
                return query;
            }

            var pageIndex = criteria.PageNumber - 1;
            if (pageIndex < 0 || criteria.PageSize < 1)
            {
                return query;
            }

            return query
                .Skip(pageIndex * criteria.PageSize)
                .Take(criteria.PageSize);
        }


    }
}
