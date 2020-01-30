using System;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Extensions;
using VivesRental.Services.Results;

namespace VivesRental.Services.Mappers
{
    public static class QueryableMappingExtensions
    {
        public static IQueryable<CustomerResult> MapToResults(this IQueryable<Customer> query)
        {
            return query.Select(customer => new CustomerResult
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                NumberOfOrders = customer.Orders.AsQueryable().Count(),
                NumberOfPendingOrders = customer.Orders.AsQueryable().Count(o => o.OrderLines.Any(ol => !ol.ReturnedAt.HasValue))
            });
        }

        public static IQueryable<ArticleResult> MapToResults(this IQueryable<Article> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(article => new ArticleResult
            {
                Id = article.Id,
                ProductId = article.ProductId,
                ProductName = article.Product.Name,
                Status = article.Status
            });
        }

        public static IQueryable<ArticleReservationResult> MapToResults(this IQueryable<ArticleReservation> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(articleReservation => new ArticleReservationResult
            {
                Id = articleReservation.Id,
                ArticleId = articleReservation.ArticleId,
                ArticleStatus = articleReservation.Article.Status,
                ProductName = articleReservation.Article.Product.Name,
                FromDateTime = articleReservation.FromDateTime,
                UntilDateTime = articleReservation.UntilDateTime,
                CustomerId = articleReservation.CustomerId,
                CustomerFirstName = articleReservation.Customer.FirstName,
                CustomerLastName = articleReservation.Customer.LastName
            });
        }

        public static IQueryable<OrderResult> MapToResults(this IQueryable<Order> query)
        {
            return query.Select(order => new OrderResult
            {
                Id = order.Id,
                CustomerFirstName = order.CustomerFirstName,
                CustomerLastName = order.CustomerLastName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhoneNumber = order.CustomerPhoneNumber,
                CreatedAt = order.CreatedAt,
                CustomerId = order.CustomerId,
                ReturnedAt = order.OrderLines.AsQueryable().All(ol => ol.ReturnedAt.HasValue)
                    ? order.OrderLines.AsQueryable().Max(ol => ol.ReturnedAt)
                    : null,
                NumberOfOrderLines = order.OrderLines.AsQueryable().Count()
            });
        }

        public static IQueryable<OrderLineResult> MapToResults(this IQueryable<OrderLine> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(orderLine => new OrderLineResult
            {
                Id = orderLine.Id,
                ArticleId = orderLine.ArticleId,
                ArticleStatus = orderLine.Article.Status,
                OrderId = orderLine.OrderId,
                ProductName = orderLine.ProductName,
                ProductDescription = orderLine.ProductDescription,
                RentedAt = orderLine.RentedAt,
                ExpiresAt = orderLine.ExpiresAt,
                ReturnedAt = orderLine.ReturnedAt
            });
        }

        public static IQueryable<ProductResult> MapToResults(this IQueryable<Product> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(product => new ProductResult
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Manufacturer = product.Manufacturer,
                Publisher = product.Publisher,
                RentalExpiresAfterDays = product.RentalExpiresAfterDays,
                NumberOfArticles = product.Articles.Count,
                NumberOfAvailableArticles = product.Articles.AsQueryable()
                    .Count(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime))
            });
        }
    }
}
