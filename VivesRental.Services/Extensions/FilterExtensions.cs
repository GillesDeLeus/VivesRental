using System.Linq;
using VivesRental.Model;
using VivesRental.Services.Filters;

namespace VivesRental.Services.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<Article> ApplyFilter(this IQueryable<Article> query, ArticleFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.ProductId.HasValue)
            {
                query = query.Where(a => a.ProductId == filter.ProductId);
            }

            return query;
        }
    }
}
