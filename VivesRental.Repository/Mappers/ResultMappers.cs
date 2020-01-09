using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.Repository.Mappers
{
    public static class ResultMappers
    {
        public static IQueryable<OrderResult> MapToResults(this IQueryable<Order> query)
        {
            return query.Select(o => new OrderResult
            {
                Id = o.Id,
                CustomerFirstName = o.CustomerFirstName,
                CustomerLastName = o.CustomerLastName,
                CustomerEmail = o.CustomerEmail,
                CustomerPhoneNumber = o.CustomerPhoneNumber,
                CreatedAt = o.CreatedAt,
                CustomerId = o.CustomerId,
                ReturnedAt = o.OrderLines.All(ol => ol.ReturnedAt.HasValue)
                    ? o.OrderLines.Max(ol => ol.ReturnedAt)
                    : null,
                NumberOfOrderLines = o.OrderLines.Count()
            });
        }

        public static IQueryable<ProductResult> MapToResults(this IQueryable<Product> query)
        {
            return query.Select(p => new ProductResult
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Manufacturer = p.Manufacturer,
                Publisher = p.Publisher,
                RentalExpiresAfterDays = p.RentalExpiresAfterDays,
                NumberOfArticles = p.Articles.Count,
                NumberOfAvailableArticles = p.Articles.Count(a => a.Status == ArticleStatus.Normal &&
                                                                  a.OrderLines.All(ol => ol.ReturnedAt.HasValue))
            });
        }
    }
}
