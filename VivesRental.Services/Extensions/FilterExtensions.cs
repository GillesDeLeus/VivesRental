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

        public static IQueryable<ArticleReservation> ApplyFilter(this IQueryable<ArticleReservation> query, ArticleReservationFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.ArticleId.HasValue)
            {
                query = query.Where(a => a.ArticleId == filter.ArticleId);
            }

            if (filter.CustomerId.HasValue)
            {
                query = query.Where(a => a.CustomerId == filter.CustomerId);
            }

            return query;
        }
    }
}
