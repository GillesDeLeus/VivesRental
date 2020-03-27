using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
    [TestClass]
    public class OrderLineServiceTests
    {
        [TestMethod]
        public async Task RentAsync_Returns_True_When_Rented()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance(nameof(RentAsync_Returns_True_When_Rented));

            var customer = CustomerFactory.CreateInvalidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            context.SaveChanges();

            var orderLineService = new OrderLineService(context);

            //Act
            var result = await orderLineService.RentAsync(order.Id, article.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RentAsync_Returns_False_When_Article_Does_Not_Exist()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance(nameof(RentAsync_Returns_False_When_Article_Does_Not_Exist));

            var customer = CustomerFactory.CreateInvalidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            context.SaveChanges();

            var orderLineService = new OrderLineService(context);

            //Act
            var result = await orderLineService.RentAsync(order.Id, Guid.NewGuid());

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RentAsync_IdList_Returns_True_When_One_Rented()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance(nameof(RentAsync_IdList_Returns_True_When_One_Rented));

            var customer = CustomerFactory.CreateInvalidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            context.SaveChanges();

            var orderLineService = new OrderLineService(context);
            var idList = new List<Guid> { article.Id };

            //Act
            var result = await orderLineService.RentAsync(order.Id, idList);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RentAsync_IdList_Returns_False_When_One_Rented_And_Does_Not_Exist()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance(nameof(RentAsync_IdList_Returns_False_When_One_Rented_And_Does_Not_Exist));

            var customer = CustomerFactory.CreateInvalidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            context.SaveChanges();

            var orderLineService = new OrderLineService(context);
            var idList = new List<Guid> { Guid.NewGuid() };

            //Act
            var result = await orderLineService.RentAsync(order.Id, idList);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RentAsync_IdList_Returns_True_When_Multiple_Rented()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance(nameof(RentAsync_IdList_Returns_True_When_Multiple_Rented));

            var customer = CustomerFactory.CreateInvalidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article1 = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article1);
            var article2 = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article2);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            context.SaveChanges();

            var orderLineService = new OrderLineService(context);
            var idList = new List<Guid> {article1.Id, article2.Id};

            //Act
            var result = await orderLineService.RentAsync(order.Id, idList);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RentAsync_IdList_Returns_False_When_Multiple_Rented_And_One_Does_Not_Exist()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance(nameof(RentAsync_IdList_Returns_False_When_Multiple_Rented_And_One_Does_Not_Exist));

            var customer = CustomerFactory.CreateInvalidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article1 = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article1);
            var article2 = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article2);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            context.SaveChanges();

            var orderLineService = new OrderLineService(context);
            var idList = new List<Guid> { article1.Id, article2.Id, Guid.NewGuid() };

            //Act
            var result = await orderLineService.RentAsync(order.Id, idList);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
