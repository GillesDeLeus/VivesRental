using System;
using VivesRental.Model;

namespace VivesRental.Tests.Data.Factories
{
    public static class ArticleReservationFactory
    {
        public static ArticleReservation CreateValidEntity(Customer customer, Article article, DateTime fromDateTime, DateTime untilDateTime)
        {
            return new ArticleReservation
            {
                CustomerId = customer.Id,
                Customer = customer,
                ArticleId = article.Id,
                Article = article,
                FromDateTime = fromDateTime,
                UntilDateTime = untilDateTime
            };
        }

        public static ArticleReservation CreateInvalidEntity()
        {
            return new ArticleReservation();
        }
    }
}
