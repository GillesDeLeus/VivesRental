using System;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Extensions;
using VivesRental.Services.Results;

namespace VivesRental.Services.Mappers
{
    //public static class EntityMappingExtensions
    //{
    //    public static Expression<Func<Article, ArticleResult>> MapToArticleResult(DateTime fromDateTime, DateTime? untilDateTime = null)
    //    {
    //        return article => new ArticleResult
    //        {
    //            Id = article.Id,
    //            ProductId = article.ProductId,
    //            //Product = article.Product.MapToResult(fromDateTime, untilDateTime),
    //            Product = MapToProductResult(fromDateTime, untilDateTime).Compile().Invoke(article.Product),
    //            Status = article.Status
    //        };
    //    }

    //    public static Expression<Func<ArticleReservation, ArticleReservationResult>> MapToArticleReservationResult(DateTime fromDateTime, DateTime? untilDateTime = null)
    //    {
    //        return articleReservation => new ArticleReservationResult
    //        {
    //            Id = articleReservation.Id,
    //            ArticleId = articleReservation.ArticleId,
    //            Article = MapToArticleResult(fromDateTime, untilDateTime).Compile().Invoke(articleReservation.Article),
    //            //Article = articleReservation.Article.MapToResult(fromDateTime, untilDateTime),
    //            FromDateTime = articleReservation.FromDateTime,
    //            UntilDateTime = articleReservation.UntilDateTime,
    //            CustomerId = articleReservation.CustomerId,
    //            //Customer = articleReservation.Customer.MapToResult()
    //            Customer = MapToCustomerResult().Compile().Invoke(articleReservation.Customer)
    //        };
    //    }

    //    public static Expression<Func<Customer, CustomerResult>> MapToCustomerResult()
    //    {
    //        return customer => new CustomerResult
    //        {
    //            Id = customer.Id,
    //            FirstName = customer.FirstName,
    //            LastName = customer.LastName,
    //            Email = customer.Email,
    //            PhoneNumber = customer.PhoneNumber,
    //            NumberOfOrders = customer.Orders.AsQueryable().Count(),
    //            NumberOfPendingOrders = customer.Orders.AsQueryable().Count(o => o.OrderLines.Any(ol => !ol.ReturnedAt.HasValue))
    //        };
    //    }

    //    public static Expression<Func<Order, OrderResult>> MapToOrderResult()
    //    {
    //        return order => new OrderResult
    //        {
    //            Id = order.Id,
    //            Customer = order.Customer.MapToResult(),
    //            CustomerFirstName = order.CustomerFirstName,
    //            CustomerLastName = order.CustomerLastName,
    //            CustomerEmail = order.CustomerEmail,
    //            CustomerPhoneNumber = order.CustomerPhoneNumber,
    //            CreatedAt = order.CreatedAt,
    //            CustomerId = order.CustomerId,
    //            ReturnedAt = order.OrderLines.AsQueryable().All(ol => ol.ReturnedAt.HasValue)
    //                ? order.OrderLines.AsQueryable().Max(ol => ol.ReturnedAt)
    //                : null,
    //            NumberOfOrderLines = order.OrderLines.AsQueryable().Count()
    //        };
    //    }

    //    public static Expression<Func<OrderLine, OrderLineResult>> MapToOrderLineResult(DateTime fromDateTime, DateTime? untilDateTime)
    //    {
    //        return orderLine => new OrderLineResult
    //        {
    //            Id = orderLine.Id,
    //            ArticleId = orderLine.ArticleId,
    //            //Article = orderLine.Article.MapToResult(fromDateTime, untilDateTime),
    //            Article = MapToArticleResult(fromDateTime,untilDateTime).Compile().Invoke(orderLine.Article),
    //            OrderId = orderLine.OrderId,
    //            //Order = orderLine.Order.MapToResult(),
    //            Order = MapToOrderResult().Compile().Invoke(orderLine.Order),
    //            ProductName = orderLine.ProductName,
    //            ProductDescription = orderLine.ProductDescription,
    //            RentedAt = orderLine.RentedAt,
    //            ExpiresAt = orderLine.ExpiresAt,
    //            ReturnedAt = orderLine.ReturnedAt
    //        };
    //    }

    //    public static Expression<Func<Product, ProductResult>> MapToProductResult(DateTime fromDateTime,
    //        DateTime? untilDateTime)
    //    {
    //        return product => new ProductResult
    //        {
    //            Id = product.Id,
    //            Name = product.Name,
    //            Description = product.Description,
    //            Manufacturer = product.Manufacturer,
    //            Publisher = product.Publisher,
    //            RentalExpiresAfterDays = product.RentalExpiresAfterDays,
    //            NumberOfArticles = product.Articles.Count,
    //            NumberOfAvailableArticles = product.Articles.AsQueryable()
    //                .Count(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime))
    //        };
    //    }


    //    //public static ArticleResult MapToResult(this Article article, DateTime fromDateTime, DateTime? untilDateTime)
    //    //{
    //    //    if (article == null)
    //    //    {
    //    //        return null;
    //    //    }

    //    //    return new ArticleResult
    //    //    {
    //    //        Id = article.Id,
    //    //        ProductId = article.ProductId,
    //    //        Product = article.Product.MapToResult(fromDateTime, untilDateTime),
    //    //        Status = article.Status
    //    //    };
    //    //}
    //    //public static ArticleReservationResult MapToResult(this ArticleReservation articleReservation, DateTime fromDateTime, DateTime? untilDateTime)
    //    //{
    //    //    if (articleReservation == null)
    //    //    {
    //    //        return null;
    //    //    }

    //    //    return new ArticleReservationResult
    //    //    {
    //    //        Id = articleReservation.Id,
    //    //        ArticleId = articleReservation.ArticleId,
    //    //        Article = MapToArticleResult(fromDateTime, untilDateTime).Compile().Invoke(articleReservation.Article),
    //    //        //Article = articleReservation.Article.MapToResult(fromDateTime, untilDateTime),
    //    //        FromDateTime = articleReservation.FromDateTime,
    //    //        UntilDateTime = articleReservation.UntilDateTime,
    //    //        CustomerId = articleReservation.CustomerId,
    //    //        Customer = articleReservation.Customer.MapToResult()
    //    //    };
    //    //}

    //    //public static CustomerResult MapToResult(this Customer customer)
    //    //{
    //    //    if (customer == null)
    //    //    {
    //    //        return null;
    //    //    }

    //    //    return new CustomerResult
    //    //    {
    //    //        Id = customer.Id,
    //    //        FirstName = customer.FirstName,
    //    //        LastName = customer.LastName,
    //    //        Email = customer.Email,
    //    //        PhoneNumber = customer.PhoneNumber,
    //    //        NumberOfOrders = customer.Orders.Count(),
    //    //        NumberOfPendingOrders = customer.Orders.Count(o => o.OrderLines.Any(ol => !ol.ReturnedAt.HasValue))
    //    //    };
    //    //}

    //    //public static OrderResult MapToResult(this Order order)
    //    //{
    //    //    if (order == null)
    //    //    {
    //    //        return null;
    //    //    }

    //    //    return new OrderResult
    //    //    {
    //    //        Id = order.Id,
    //    //        Customer = order.Customer.MapToResult(),
    //    //        CustomerFirstName = order.CustomerFirstName,
    //    //        CustomerLastName = order.CustomerLastName,
    //    //        CustomerEmail = order.CustomerEmail,
    //    //        CustomerPhoneNumber = order.CustomerPhoneNumber,
    //    //        CreatedAt = order.CreatedAt,
    //    //        CustomerId = order.CustomerId,
    //    //        ReturnedAt = order.OrderLines.All(ol => ol.ReturnedAt.HasValue)
    //    //            ? order.OrderLines.Max(ol => ol.ReturnedAt)
    //    //            : null,
    //    //        NumberOfOrderLines = order.OrderLines.Count()
    //    //    };
    //    //}

    //    //public static OrderLineResult MapToResult(this OrderLine orderLine, DateTime fromDateTime, DateTime? untilDateTime)
    //    //{
    //    //    if (orderLine == null)
    //    //    {
    //    //        return null;
    //    //    }

    //    //    return new OrderLineResult
    //    //    {
    //    //        Id = orderLine.Id,
    //    //        ArticleId = orderLine.ArticleId,
    //    //        Article = orderLine.Article.MapToResult(fromDateTime, untilDateTime),
    //    //        OrderId = orderLine.OrderId,
    //    //        Order = orderLine.Order.MapToResult(),
    //    //        ProductName = orderLine.ProductName,
    //    //        ProductDescription = orderLine.ProductDescription,
    //    //        RentedAt = orderLine.RentedAt,
    //    //        ExpiresAt = orderLine.ExpiresAt,
    //    //        ReturnedAt = orderLine.ReturnedAt
    //    //    };
    //    //}

    //    //public static ProductResult MapToResult(this Product product, DateTime fromDateTime, DateTime? untilDateTime)
    //    //{
    //    //    if (product == null)
    //    //    {
    //    //        return null;
    //    //    }

    //    //    return new ProductResult
    //    //    {
    //    //        Id = product.Id,
    //    //        Name = product.Name,
    //    //        Description = product.Description,
    //    //        Manufacturer = product.Manufacturer,
    //    //        Publisher = product.Publisher,
    //    //        RentalExpiresAfterDays = product.RentalExpiresAfterDays,
    //    //        NumberOfArticles = product.Articles.Count,
    //    //        NumberOfAvailableArticles = product.Articles.AsQueryable()
    //    //            .Count(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime))
    //    //    };
    //    //}

    //    [ReplaceWithExpression(MethodName = nameof(MapToArticleResult))]
    //    public static ArticleResult MapToResult(this Article article, DateTime fromDateTime, DateTime? untilDateTime)
    //    {
    //        if (article == null)
    //        {
    //            return null;
    //        }

    //        return MapToArticleResult(fromDateTime,untilDateTime).Compile().Invoke(article);
    //    }

    //    [ReplaceWithExpression(MethodName = nameof(MapToArticleReservationResult))]
    //    public static ArticleReservationResult MapToResult(this ArticleReservation articleReservation, DateTime fromDateTime, DateTime? untilDateTime)
    //    {
    //        if (articleReservation == null)
    //        {
    //            return null;
    //        }

    //        return MapToArticleReservationResult(fromDateTime,untilDateTime).Compile().Invoke(articleReservation);
    //    }

    //    [ReplaceWithExpression(MethodName = nameof(MapToCustomerResult))]
    //    public static CustomerResult MapToResult(this Customer customer)
    //    {
    //        if (customer == null)
    //        {
    //            return null;
    //        }

    //        return MapToCustomerResult().Compile().Invoke(customer);
    //    }

    //    [ReplaceWithExpression(MethodName = nameof(MapToOrderResult))]
    //    public static OrderResult MapToResult(this Order order)
    //    {
    //        if (order == null)
    //        {
    //            return null;
    //        }

    //        return MapToOrderResult().Compile().Invoke(order);
    //    }

    //    [ReplaceWithExpression(MethodName = nameof(MapToOrderLineResult))]
    //    public static OrderLineResult MapToResult(this OrderLine orderLine, DateTime fromDateTime, DateTime? untilDateTime)
    //    {
    //        if (orderLine == null)
    //        {
    //            return null;
    //        }

    //        return MapToOrderLineResult(fromDateTime, untilDateTime).Compile().Invoke(orderLine);
    //    }

    //    [ReplaceWithExpression(MethodName = nameof(MapToProductResult))]
    //    public static ProductResult MapToResult(this Product product, DateTime fromDateTime, DateTime? untilDateTime)
    //    {
    //        if (product == null)
    //        {
    //            return null;
    //        }

    //        return MapToProductResult(fromDateTime,untilDateTime).Compile().Invoke(product);
    //    }
    //}
}
