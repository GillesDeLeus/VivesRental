using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
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
            var productToAdd = ProductFactory.CreateValidEntity();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var productRepositoryMock = new Mock<IProductRepository>();
            
            //Setup ProductRepository
            productRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ProductIncludes>())).Returns(productToAdd);
            productRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            unitOfWorkMock.Setup(uow => uow.Products).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);
            
            var productService = new ProductService(unitOfWorkMock.Object);

            //Act
            var result = productService.Remove(productToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_Returns_False_When_Product_Is_Null()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var productRepositoryMock = new Mock<IProductRepository>();

            //Setup ProductRepository
            productRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ProductIncludes>())).Returns((Product)null);
            productRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            unitOfWorkMock.Setup(uow => uow.Products).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var productService = new ProductService(unitOfWorkMock.Object);

            //Act
            var result = productService.Remove(Guid.NewGuid());

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Remove_Deletes_Product_With_Articles()
        {
            //Arrange
            var productToAdd = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(productToAdd);
            productToAdd.Articles.Add(article);


            //Setup ProductRepository
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ProductIncludes>())).Returns(productToAdd);
            productRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup ArticleRepository
            var articleRepositoryMock = new Mock<IArticleRepository>();
            articleRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Products).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Articles).Returns(articleRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var productService = new ProductService(unitOfWorkMock.Object);

            //Act
            var result = productService.Remove(productToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_Deletes_Product_With_Articles_And_OrderLines()
        {
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            var productToAdd = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(productToAdd);
            var order = OrderFactory.CreateValidEntity(customer);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);

            article.OrderLines.Add(orderLine);
            productToAdd.Articles.Add(article);


            //Setup ProductRepository
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ProductIncludes>())).Returns(productToAdd);
            productRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup ArticleRepository
            var articleRepositoryMock = new Mock<IArticleRepository>();
            articleRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));

            //Setup OrderLineRepository
            var orderLineRepositoryMock = new Mock<IOrderLineRepository>();

            //Setup UnitOfWork
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Products).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Articles).Returns(articleRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.OrderLines).Returns(orderLineRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var productService = new ProductService(unitOfWorkMock.Object);

            //Act
            var result = productService.Remove(productToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
