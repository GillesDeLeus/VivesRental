using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Extensions;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public void Remove_Deletes_Product()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product");
            using var unitOfWork = UnitOfWorkFactory.CreateInstance(context);

            var product = ProductFactory.CreateValidEntity();
            unitOfWork.Add(product);
            unitOfWork.Complete();

            var productService = new ProductService(unitOfWork);

            //Act
            var result = productService.Remove(product.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_Returns_False_When_Product_Is_Null()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Returns_False_When_Product_Is_Null");
            using var unitOfWork = UnitOfWorkFactory.CreateInstance(context);

            var productService = new ProductService(unitOfWork);

            //Act
            var result = productService.Remove(Guid.NewGuid());

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Remove_Deletes_Product_With_Articles()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles");
            using var unitOfWork = UnitOfWorkFactory.CreateInstance(context);
            
            var productToAdd = ProductFactory.CreateValidEntity();
            unitOfWork.Add(productToAdd);
            var article = ArticleFactory.CreateValidEntity(productToAdd);
            unitOfWork.Add(article);
            unitOfWork.Complete();

            var productService = new ProductService(unitOfWork);

            //Act
            var result = productService.Remove(productToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_Deletes_Product_With_Articles_And_OrderLines()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles_And_OrderLines");
            using var unitOfWork = UnitOfWorkFactory.CreateInstance(context);

            var customer = CustomerFactory.CreateValidEntity();
            unitOfWork.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            unitOfWork.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article);
            var order = OrderFactory.CreateValidEntity(customer);
            unitOfWork.Add(order);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);
            unitOfWork.Add(orderLine);
            unitOfWork.Complete();

            var productService = new ProductService(unitOfWork);

            //Act
            var result = productService.Remove(product.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetAvailableProductResults_Returns_Available_Product()
        {
            var context = DbContextFactory.CreateInstance("GetAvailableProductResults_Returns_Available_Product");
            var unitOfWork = UnitOfWorkFactory.CreateInstance(context);

            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            unitOfWork.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            unitOfWork.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article);
            var article2 = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article2);
            unitOfWork.Complete();

            var productService = new ProductService(unitOfWork);

            //Act
            var result = productService.All();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetAvailableProductResults_Returns_Available_Product_WithOrderLine()
        {
            var context = DbContextFactory.CreateInstance("GetAvailableProductResults_Returns_Available_Product_WithOrderLine");
            var unitOfWork = UnitOfWorkFactory.CreateInstance(context);

            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            unitOfWork.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            unitOfWork.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article);
            var article2 = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article2);
            var order = OrderFactory.CreateValidEntity(customer);
            unitOfWork.Add(order);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);
            unitOfWork.Add(orderLine);
            unitOfWork.Complete();

            var productService = new ProductService(unitOfWork);

            //Act
            var result = productService.GetAvailableProductResults();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetAvailableProductResults_Returns_No_Available_Product_When_All_Rented()
        {
            var context = DbContextFactory.CreateInstance("GetAvailableProductResults_Returns_No_Available_Product_When_All_Rented");
            var unitOfWork = UnitOfWorkFactory.CreateInstance(context);

            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            unitOfWork.Add(customer);
            var product = ProductFactory.CreateValidEntity();
            unitOfWork.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article);
            var article2 = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article2);
            var order = OrderFactory.CreateValidEntity(customer);
            unitOfWork.Add(order);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);
            unitOfWork.Add(orderLine);
            var orderLine2 = OrderLineFactory.CreateValidEntity(order, article2);
            unitOfWork.Add(orderLine2);
            unitOfWork.Complete();

            var productService = new ProductService(unitOfWork);

            //Act
            var result = productService.GetAvailableProductResults();

            Assert.AreEqual(0, result.Count);
        }
    }
}
