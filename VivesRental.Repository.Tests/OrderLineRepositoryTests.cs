using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class OrderLineRepositoryTests
    {
        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_Order()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productRepository = new ProductRepository(context);
                var articleRepository = new ArticleRepository(context);
                var customerRepository = new CustomerRepository(context);
                var orderRepository = new OrderRepository(context);
                var orderLineRepository = new OrderLineRepository(context);

                //Act
                var product = ProductFactory.CreateValidEntity();
                productRepository.Add(product);
                var article = ArticleFactory.CreateValidEntity(product);
                articleRepository.Add(article);
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var order = OrderFactory.CreateValidEntity(customer);
                orderRepository.Add(order);
                var orderLine = OrderLineFactory.CreateValidEntity(order, article);
                orderLineRepository.Add(orderLine);

                var result = context.SaveChanges();

                //Assert
                Assert.AreEqual(5, result); //Because we added five entities
            }
        }

        [TestMethod]
        public void Get_Returns_Null_When_Not_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var orderRepository = new OrderRepository(context);

                //Act
                var order = orderRepository.Get(Guid.NewGuid());

                //Assert
                Assert.IsNull(order);
            }
        }

        [TestMethod]
        public void Get_Returns_Order_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productRepository = new ProductRepository(context);
                var articleRepository = new ArticleRepository(context);
                var customerRepository = new CustomerRepository(context);
                var orderRepository = new OrderRepository(context);
                var orderLineRepository = new OrderLineRepository(context);

                var product = ProductFactory.CreateValidEntity();
                productRepository.Add(product);
                var article = ArticleFactory.CreateValidEntity(product);
                articleRepository.Add(article);
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var order = OrderFactory.CreateValidEntity(customer);
                orderRepository.Add(order);
                var orderLineToAdd = OrderLineFactory.CreateValidEntity(order, article);
                orderLineRepository.Add(orderLineToAdd);

                context.SaveChanges();

                //Act
                var orderLine = orderLineRepository.Get(orderLineToAdd.Id);

                //Assert
                Assert.IsNotNull(orderLine);
            }
        }
    }
}
