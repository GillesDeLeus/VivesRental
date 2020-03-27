using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public async Task Remove_Deletes_Product()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product");
            
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            context.SaveChanges();

            var productService = new ProductService(context);

            //Act
            var result = await productService.RemoveAsync(product.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Remove_Deletes_Product_With_Articles()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles");
            
            var productToAdd = ProductFactory.CreateValidEntity();
            context.Products.Add(productToAdd);
            var article = ArticleFactory.CreateValidEntity(productToAdd);
            context.Articles.Add(article);
            context.SaveChanges();

            var productService = new ProductService(context);

            //Act
            var result = await productService.RemoveAsync(productToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Remove_Deletes_Product_With_Articles_And_OrderLines()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles_And_OrderLines");
            
            var customer = CustomerFactory.CreateValidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);
            context.OrderLines.Add(orderLine);
            context.SaveChanges();

            var productService = new ProductService(context);

            //Act
            var result = await productService.RemoveAsync(product.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetAvailableProductResults_Returns_Available_Product()
        {
            var context = DbContextFactory.CreateInstance("GetAvailableProductResults_Returns_Available_Product");
            
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            var article2 = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article2);
            context.SaveChanges();

            var productService = new ProductService(context);

            //Act
            var result = await productService.AllAsync();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAvailableProductResults_Returns_Available_Product_WithOrderLine()
        {
            var context = DbContextFactory.CreateInstance("GetAvailableProductResults_Returns_Available_Product_WithOrderLine");
            
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            var article2 = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article2);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);
            context.OrderLines.Add(orderLine);
            context.SaveChanges();

            var productService = new ProductService(context);

            //Act
            var result = await productService.GetAvailableProductResultsAsync();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAvailableProductResults_Returns_No_Available_Product_When_All_Rented()
        {
            var context = DbContextFactory.CreateInstance("GetAvailableProductResults_Returns_No_Available_Product_When_All_Rented");
            
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            context.Customers.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            var article2 = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article2);
            var order = OrderFactory.CreateValidEntity(customer);
            context.Orders.Add(order);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);
            context.OrderLines.Add(orderLine);
            var orderLine2 = OrderLineFactory.CreateValidEntity(order, article2);
            context.OrderLines.Add(orderLine2);
            context.SaveChanges();

            var productService = new ProductService(context);

            //Act
            var result = await productService.GetAvailableProductResultsAsync();

            Assert.AreEqual(0, result.Count);
        }
    }
}
