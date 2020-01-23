using System;
using VivesRental.Model;
using VivesRental.Repository.Core;

namespace VivesRental.Tests.Data.Extensions
{
    public static class VivesRentalDbContextExtensions
    {
        public static void Add(this IVivesRentalDbContext context, Customer customer)
        {
            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }
            context.Customers.Add(customer);
        }

        public static void Add(this IVivesRentalDbContext context, Product product)
        {
            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.NewGuid();
            }
            context.Products.Add(product);
        }

        public static void Add(this IVivesRentalDbContext context, Article article)
        {
            if (article.Id == Guid.Empty)
            {
                article.Id = Guid.NewGuid();
            }
            context.Articles.Add(article);
        }

        public static void Add(this IVivesRentalDbContext context, ArticleReservation articleReservation)
        {
            if (articleReservation.Id == Guid.Empty)
            {
                articleReservation.Id = Guid.NewGuid();
            }
            context.ArticleReservations.Add(articleReservation);
        }

        public static void Add(this IVivesRentalDbContext context, Order order)
        {
            if (order.Id == Guid.Empty)
            {
                order.Id = Guid.NewGuid();
            }
            context.Orders.Add(order);
        }

        public static void Add(this IVivesRentalDbContext context, OrderLine orderLine)
        {
            if (orderLine.Id == Guid.Empty)
            {
                orderLine.Id = Guid.NewGuid();
            }
            context.OrderLines.Add(orderLine);
        }
    }
}
