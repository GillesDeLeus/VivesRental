using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Tests.Data.Extensions;
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
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles");
            using var unitOfWork = UnitOfWorkFactory.CreateInstance(context);

            var product = ProductFactory.CreateValidEntity();
            unitOfWork.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            unitOfWork.Add(article);
            unitOfWork.Complete();

            var articleService = new ArticleService(unitOfWork);

            //Act
            var result = articleService.Remove(article.Id);

            //Assert
            Assert.IsTrue(result);
        }

		[TestMethod]
		public void Remove_Deletes_Article_With_OrderLines()
		{
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles");
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

            var articleService = new ArticleService(unitOfWork);

            //Act
            var result = articleService.Remove(article.Id);

            //Assert
            Assert.IsTrue(result);
        }
	}
}
