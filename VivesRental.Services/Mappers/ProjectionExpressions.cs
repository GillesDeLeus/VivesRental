﻿using System;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Extensions;
using VivesRental.Services.Results;

namespace VivesRental.Services.Mappers
{
    public static class ProjectionExpressions
    {
        public static Expression<Func<Article, ArticleResult>> ProjectToArticleResult(DateTime fromDateTime, DateTime? untilDateTime)
        {
            return entity => new ArticleResult
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                Product = entity.Product.MapToResult(fromDateTime, untilDateTime),
                Status = entity.Status
            };
        }

        public static Expression<Func<ArticleReservation, ArticleReservationResult>> ProjectToArticleReservationResult(
            DateTime fromDateTime, DateTime? untilDateTime)
        {
            return entity => new ArticleReservationResult
            {
                Id = entity.Id,
                ArticleId = entity.ArticleId,
                Article = entity.Article.MapToResult(fromDateTime, untilDateTime),
                FromDateTime = entity.FromDateTime,
                UntilDateTime = entity.UntilDateTime,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer.MapToResult()
            };
        }

        public static Expression<Func<Customer, CustomerResult>> ProjectToCustomerResult()
        {
            return entity => new CustomerResult
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                NumberOfOrders = entity.Orders.Count,
                NumberOfPendingOrders = entity.Orders.Count(o => o.OrderLines.Any(ol => !ol.ReturnedAt.HasValue))
            };
        }

        public static Expression<Func<Order, OrderResult>> ProjectToOrderResult()
        {
            return entity => new OrderResult
            {
                Id = entity.Id,
                Customer = entity.Customer.MapToResult(),
                CustomerFirstName = entity.CustomerFirstName,
                CustomerLastName = entity.CustomerLastName,
                CustomerEmail = entity.CustomerEmail,
                CustomerPhoneNumber = entity.CustomerPhoneNumber,
                CreatedAt = entity.CreatedAt,
                CustomerId = entity.CustomerId,
                ReturnedAt = entity.OrderLines.All(ol => ol.ReturnedAt.HasValue)
                    ? entity.OrderLines.Max(ol => ol.ReturnedAt)
                    : null,
                NumberOfOrderLines = entity.OrderLines.Count()
            };
        }

        public static Expression<Func<OrderLine, OrderLineResult>> ProjectToOrderLineResult(DateTime fromDateTime, DateTime? untilDateTime)
        {
            return entity => new OrderLineResult
            {
                Id = entity.Id,
                ArticleId = entity.ArticleId,
                Article = entity.Article.MapToResult(fromDateTime, untilDateTime),
                OrderId = entity.OrderId,
                Order = entity.Order.MapToResult(),
                ProductName = entity.ProductName,
                ProductDescription = entity.ProductDescription,
                RentedAt = entity.RentedAt,
                ExpiresAt = entity.ExpiresAt,
                ReturnedAt = entity.ReturnedAt
            };
        }

        public static Expression<Func<Product, ProductResult>> ProjectToProductResult(DateTime fromDateTime, DateTime? untilDateTime)
        {
            return entity => new ProductResult
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Manufacturer = entity.Manufacturer,
                Publisher = entity.Publisher,
                RentalExpiresAfterDays = entity.RentalExpiresAfterDays,
                NumberOfArticles = entity.Articles.Count,
                NumberOfAvailableArticles = entity.Articles.AsQueryable()
                    .Count(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime))
            };
        }
    }
}