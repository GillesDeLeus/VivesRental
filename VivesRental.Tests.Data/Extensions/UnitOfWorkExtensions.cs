using System;
using VivesRental.Model;
using VivesRental.Repository.Core;

namespace VivesRental.Tests.Data.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static void Add(this IUnitOfWork unitOfWork, Customer customer)
        {
            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }
            unitOfWork.Customers.Add(customer);
        }

        public static void Add(this IUnitOfWork unitOfWork, Product product)
        {
            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.NewGuid();
            }
            unitOfWork.Products.Add(product);
        }

        public static void Add(this IUnitOfWork unitOfWork, Article article)
        {
            if (article.Id == Guid.Empty)
            {
                article.Id = Guid.NewGuid();
            }
            unitOfWork.Articles.Add(article);
        }

        public static void Add(this IUnitOfWork unitOfWork, ArticleReservation articleReservation)
        {
            if (articleReservation.Id == Guid.Empty)
            {
                articleReservation.Id = Guid.NewGuid();
            }
            unitOfWork.ArticleReservations.Add(articleReservation);
        }

        public static void Add(this IUnitOfWork unitOfWork, Order order)
        {
            if (order.Id == Guid.Empty)
            {
                order.Id = Guid.NewGuid();
            }
            unitOfWork.Orders.Add(order);
        }

        public static void Add(this IUnitOfWork unitOfWork, OrderLine orderLine)
        {
            if (orderLine.Id == Guid.Empty)
            {
                orderLine.Id = Guid.NewGuid();
            }
            unitOfWork.OrderLines.Add(orderLine);
        }
    }
}
