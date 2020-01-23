using System.Linq;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Extensions
{
    public static class IncludeExtensions
    {
        /// <summary>
        /// Adds the DbContext includes based on the booleans set in the Includes object
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static IQueryable<Article> AddIncludes(this IQueryable<Article> query, ArticleIncludes includes)
        {
            if (includes == null)
                return query;

            if (includes.Product)
                query = query.Include(i => i.Product);

            return query;
        }

        /// <summary>
        /// Adds the DbContext includes based on the booleans set in the Includes object
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static IQueryable<ArticleReservation> AddIncludes(this IQueryable<ArticleReservation> query, ArticleReservationIncludes includes)
        {
            if (includes == null)
                return query;

            if (includes.Article)
                query = query.Include(i => i.Article);

            if (includes.Customer)
                query = query.Include(i => i.Customer);

            return query;
        }

        /// <summary>
        /// Adds the DbContext includes based on the booleans set in the Includes object
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static IQueryable<Order> AddIncludes(this IQueryable<Order> query, OrderIncludes includes)
        {
            if (includes == null)
            {
                return query;
            }

            if (includes.Customer)
            {
                query = query.Include(i => i.Customer);
            }

            return query;
        }

        /// <summary>
        /// Adds the DbContext includes based on the booleans set in the Includes object
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static IQueryable<OrderLine> AddIncludes(this IQueryable<OrderLine> query, OrderLineIncludes includes)
        {
            if (includes == null)
            {
                return query;
            }

            if (includes.OrderCustomer)
            {
                query = query.Include(i => i.Order).ThenInclude(o => o.Customer);
            }

            if (includes.ArticleProduct)
            {
                query = query.Include(i => i.Article).ThenInclude(a => a.Product);
            }

            return query;
        }
    }
}
