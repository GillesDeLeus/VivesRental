using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;

namespace VivesRental.Repository.Extensions
{
    public static class ArticleExtensions
    {
        public static Func<Article, bool> IsAvailable(DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return article => article.Status == ArticleStatus.Normal && //Only articles that are "Normal"
                              article.OrderLines.All(ol => ol.ReturnedAt.HasValue) && //Only articles that are in stock
                              !article.ArticleReservations.Any(IsReserved(fromDateTime, untilDateTime));
        }

        public static Func<Article, bool> IsAvailable(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return article => article.Id == articleId &&
                              article.Status == ArticleStatus.Normal && //Only articles that are "Normal"
                              article.OrderLines.All(ol => ol.ReturnedAt.HasValue) && //Only articles that are in stock
                              !article.ArticleReservations.Any(IsReserved(fromDateTime, untilDateTime));
        }
        
        public static Func<Article, bool> IsAvailable(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return article => articleIds.Contains(article.Id) &&
                              article.Status == ArticleStatus.Normal && //Only articles that are "Normal"
                              article.OrderLines.All(ol => ol.ReturnedAt.HasValue) && //Only articles that are in stock
                              !article.ArticleReservations.Any(IsReserved(fromDateTime, untilDateTime));
        }

        public static Func<ArticleReservation, bool> IsReserved(DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return ar =>
                (untilDateTime.HasValue && ar.FromDateTime < untilDateTime || ar.FromDateTime <
                 fromDateTime.AddDays(ar.Article.Product.RentalExpiresAfterDays)
                ) //If we do not have an UntilDateTime, just add the expiry days to the FromDate
                && ar.UntilDateTime > fromDateTime;
        }
        
        public static bool IsRented(this Article article)
        {
            return article.Status == ArticleStatus.Normal && //Only articles that are "Normal"
                   article.OrderLines.Any(rol => !rol.ReturnedAt.HasValue); //Only articles that are rented
        }

        public static bool IsRented(this Article article, Guid customerId)
        {
            return article.Status == ArticleStatus.Normal && //Only articles that are "Normal"
                   article.OrderLines.Any(rol => !rol.ReturnedAt.HasValue && rol.Order.CustomerId == customerId); //Only articles that are rented for a customer
        }
    }
}
