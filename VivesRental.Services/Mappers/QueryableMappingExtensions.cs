using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Services.Results;

namespace VivesRental.Services.Mappers
{
    public static class QueryableMappingExtensions
    {
        public static IEnumerable<CustomerResult> MapToResults(this IEnumerable<Customer> query)
        {
            return query.Select(o => o.MapToResult());
        }

        public static IEnumerable<ArticleResult> MapToResults(this IEnumerable<Article> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(o => o.MapToResult(fromDateTime, untilDateTime));
        }

        public static IEnumerable<ArticleReservationResult> MapToResults(this IEnumerable<ArticleReservation> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(o => o.MapToResult(fromDateTime, untilDateTime));
        }

        public static IEnumerable<OrderResult> MapToResults(this IEnumerable<Order> query)
        {
            return query.Select(o => o.MapToResult());
        }

        public static IEnumerable<OrderLineResult> MapToResults(this IEnumerable<OrderLine> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(o => o.MapToResult(fromDateTime, untilDateTime));
        }

        public static IEnumerable<ProductResult> MapToResults(this IEnumerable<Product> query, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return query.Select(p => p.MapToResult(fromDateTime, untilDateTime));
        }

        
    }
}
