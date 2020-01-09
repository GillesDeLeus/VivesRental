using System;
using System.Linq;
using VivesRental.Model;

namespace VivesRental.Repository.Extensions
{
    public static class ArticleExtensions
    {
        public static bool IsAvailable(this Article article, DateTime fromDateTime)
        {
            return article.IsAvailable(fromDateTime, null);
        }
        public static bool IsAvailable(this Article article, DateTime fromDateTime, DateTime? untilDateTime)
        {
            return article.Status == ArticleStatus.Normal && //Only articles that are "Normal"
                   article.OrderLines.All(ol => ol.ReturnedAt.HasValue) && //Only articles that are in stock
                   !article.ArticleReservations.Any(ar =>
                       (untilDateTime.HasValue && ar.FromDateTime < untilDateTime || ar.FromDateTime < fromDateTime.AddDays(ar.Article.Product.RentalExpiresAfterDays)) //If we do not have an UntilDateTime, just add the expiry days to the FromDate
                       && ar.UntilDateTime > fromDateTime);
        }

        public static bool IsRented(this Article article)
        {
            return article.Status == ArticleStatus.Normal &&
                   article.OrderLines.Any(rol => !rol.ReturnedAt.HasValue);
        }

        public static bool IsRented(this Article article, Guid customerId)
        {
            return article.Status == ArticleStatus.Normal &&
                   article.OrderLines.Any(rol => !rol.ReturnedAt.HasValue && rol.Order.CustomerId == customerId);
        }
    }
}
