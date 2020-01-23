using System;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Extensions;
using VivesRental.Services.Results;

namespace VivesRental.Services.Mappers
{
    public static class EntityMappingExtensions
    {
        public static ArticleResult MapToResult(this Article article, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (article == null)
            {
                return null;
            }

            return new ArticleResult
            {
                Id = article.Id,
                ProductId = article.ProductId,
                Product = article.Product.MapToResult(fromDateTime, untilDateTime),
                Status = article.Status
            };
        }
        public static ArticleReservationResult MapToResult(this ArticleReservation articleReservation, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (articleReservation == null)
            {
                return null;
            }

            return new ArticleReservationResult
            {
                Id = articleReservation.Id,
                ArticleId = articleReservation.ArticleId,
                Article = articleReservation.Article.MapToResult(fromDateTime, untilDateTime),
                FromDateTime = articleReservation.FromDateTime,
                UntilDateTime = articleReservation.UntilDateTime,
                CustomerId = articleReservation.CustomerId,
                Customer = articleReservation.Customer.MapToResult()
            };
        }

        public static CustomerResult MapToResult(this Customer customer)
        {
            if (customer == null)
            {
                return null;
            }

            return new CustomerResult
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                NumberOfOrders = customer.Orders.Count(),
                NumberOfPendingOrders = customer.Orders.Count(o => o.OrderLines.Any(ol => !ol.ReturnedAt.HasValue))
            };
        }

        public static OrderResult MapToResult(this Order order)
        {
            if (order == null)
            {
                return null;
            }

            return new OrderResult
            {
                Id = order.Id,
                Customer = order.Customer.MapToResult(),
                CustomerFirstName = order.CustomerFirstName,
                CustomerLastName = order.CustomerLastName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhoneNumber = order.CustomerPhoneNumber,
                CreatedAt = order.CreatedAt,
                CustomerId = order.CustomerId,
                ReturnedAt = order.OrderLines.All(ol => ol.ReturnedAt.HasValue)
                    ? order.OrderLines.Max(ol => ol.ReturnedAt)
                    : null,
                NumberOfOrderLines = order.OrderLines.Count()
            };
        }

        public static OrderLineResult MapToResult(this OrderLine orderLine, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (orderLine == null)
            {
                return null;
            }

            return new OrderLineResult
            {
                Id = orderLine.Id,
                ArticleId = orderLine.ArticleId,
                Article = orderLine.Article.MapToResult(fromDateTime, untilDateTime),
                OrderId = orderLine.OrderId,
                Order = orderLine.Order.MapToResult(),
                ProductName = orderLine.ProductName,
                ProductDescription = orderLine.ProductDescription,
                RentedAt = orderLine.RentedAt,
                ExpiresAt = orderLine.ExpiresAt,
                ReturnedAt = orderLine.ReturnedAt
            };
        }

        public static ProductResult MapToResult(this Product product, DateTime fromDateTime, DateTime? untilDateTime)
        {
            if (product == null)
            {
                return null;
            }

            return new ProductResult
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
            };
        }
    }
}
