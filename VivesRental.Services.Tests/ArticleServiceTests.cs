using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
	[TestClass]
	public class ArticleServiceTests
	{
		[TestMethod]
		public void Remove_Deletes_Article()
        {
            //Arrange
            var productToAdd = ProductFactory.CreateValidEntity();
            var articleToAdd = ArticleFactory.CreateValidEntity(productToAdd);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var articleRepositoryMock = new Mock<IArticleRepository>();

            //Setup ArticleRepository
            articleRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ArticleIncludes>())).Returns(articleToAdd);
            articleRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            unitOfWorkMock.Setup(uow => uow.Articles).Returns(articleRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var articleService = new ArticleService(unitOfWorkMock.Object);

            //Act
            var result = articleService.Remove(articleToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

		[TestMethod]
		public void Remove_Deletes_Article_With_OrderLines()
		{
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            var productToAdd = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(productToAdd);
            var order = OrderFactory.CreateValidEntity(customer);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);

            article.OrderLines.Add(orderLine);
            productToAdd.Articles.Add(article);

            //Setup ArticleRepository
            var articleRepositoryMock = new Mock<IArticleRepository>();
            articleRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ArticleIncludes>())).Returns(article);
            articleRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Articles).Returns(articleRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var articleService = new ArticleService(unitOfWorkMock.Object);

            //Act
            var result = articleService.Remove(productToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }
	}
}
