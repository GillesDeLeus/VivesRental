using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Tests.Data.Extensions;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
	[TestClass]
	public class ArticleServiceTests
	{
		[TestMethod]
		public async Task Remove_Deletes_Article()
        {
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles");
            
            var product = ProductFactory.CreateValidEntity();
            context.Products.Add(product);
            var article = ArticleFactory.CreateValidEntity(product);
            context.Articles.Add(article);
            context.SaveChanges();

            var articleService = new ArticleService(context);

            //Act
            var result = await articleService.RemoveAsync(article.Id);

            //Assert
            Assert.IsTrue(result);
        }

		[TestMethod]
		public async Task Remove_Deletes_Article_With_OrderLines()
		{
            //Arrange
            using var context = DbContextFactory.CreateInstance("Remove_Deletes_Product_With_Articles");
            
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
            await context.SaveChangesAsync();

            var articleService = new ArticleService(context);

            //Act
            var result = await articleService.RemoveAsync(article.Id);

            //Assert
            Assert.IsTrue(result);
        }
	}
}
