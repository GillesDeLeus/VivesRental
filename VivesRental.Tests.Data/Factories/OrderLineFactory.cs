using System;
using VivesRental.Model;

namespace VivesRental.Tests.Data.Factories
{
    public static class OrderLineFactory
    {
        public static OrderLine CreateValidEntity(Order order, Article article)
        {
            return new OrderLine
            {
                OrderId =  order.Id,
                Order = order,
                ArticleId = article.Id,
                Article = article,
                ProductName = "TestProductName",
                ProductDescription = "TestProductDescription",
                RentedAt = DateTime.Now,
                ExpiresAt = DateTime.Now,
                ReturnedAt = DateTime.Now
            };
        }

        public static OrderLine CreateInvalidEntity()
        {
            return new OrderLine
            {
                RentedAt = DateTime.Now
            };
        }
    }
}
