﻿using System;
using System.Linq;
using VivesRental.Model;
using VivesRental.Services.Results;

namespace VivesRental.Services.Mappers
{
    public static class QueryableMappingExtensions
    {
        public static IQueryable<CustomerResult> MapToResults(this IQueryable<Customer> query)
        {
            return query.Select(ProjectionExpressions.ProjectToCustomerResult());
        }

        public static IQueryable<ArticleResult> MapToResults(this IQueryable<Article> query)
        {
            return query.Select(ProjectionExpressions.ProjectToArticleResult());
        }

        public static IQueryable<ArticleReservationResult> MapToResults(this IQueryable<ArticleReservation> query)
        {
            return query.Select(ProjectionExpressions.ProjectToArticleReservationResult());
        }

        public static IQueryable<OrderResult> MapToResults(this IQueryable<Order> query)
        {
            return query.Select(ProjectionExpressions.ProjectToOrderResult());
        }

        public static IQueryable<OrderLineResult> MapToResults(this IQueryable<OrderLine> query)
        {
            return query.Select(ProjectionExpressions.ProjectToOrderLineResult());
        }

        public static IQueryable<ProductResult> MapToResults(this IQueryable<Product> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(ProjectionExpressions.ProjectToProductResult(fromDateTime, untilDateTime));
        }


    }
}
