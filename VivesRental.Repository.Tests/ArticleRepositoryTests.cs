using System;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class ArticleRepositoryTests
    {
        
        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_Product()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productRepository = new ProductRepository(context);
                var articleRepository = new ArticleRepository(context);

                //Act
                var product = ProductFactory.CreateValidEntity();
                productRepository.Add(product);
                var article = ArticleFactory.CreateValidEntity(product);
                articleRepository.Add(article);

                var result = context.SaveChanges();

                //Assert
                Assert.AreEqual(2, result); //Because we added two entities
            }
        }

        [TestMethod]
        public void Get_Returns_Null_When_Not_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var articleRepository = new ArticleRepository(context);

                //Act
                var article = articleRepository.Get(Guid.NewGuid());

                //Assert
                Assert.IsNull(article);
            }
        }

        [TestMethod]
        public void Get_Returns_Product_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productRepository = new ProductRepository(context);
                var articleRepository = new ArticleRepository(context);

                var product = ProductFactory.CreateValidEntity();
                productRepository.Add(product);
                var articleToAdd = ArticleFactory.CreateValidEntity(product);
                articleRepository.Add(articleToAdd);
                context.SaveChanges();

                //Act
                var article = articleRepository.Get(articleToAdd.Id);

                //Assert
                Assert.IsNotNull(article);
            }
        }

        [TestMethod]
        public void GetAll_Returns_10_Products()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productRepository = new ProductRepository(context);
                var articleRepository = new ArticleRepository(context);
                for (int i = 0; i < 10; i++)
                {
                    var product = ProductFactory.CreateValidEntity();
                    productRepository.Add(product);
                    var articleToAdd = ArticleFactory.CreateValidEntity(product);
                    articleRepository.Add(articleToAdd);
                }
                context.SaveChanges();

                //Act
                var articles = articleRepository.GetAll();

                //Assert
                Assert.AreEqual(10, articles.Count());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void Remove_Throws_Exception_When_Not_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productRepository = new ProductRepository(context);
                var articleRepository = new ArticleRepository(context);

                var product = ProductFactory.CreateValidEntity();
                productRepository.Add(product);
                var articleToAdd = ArticleFactory.CreateValidEntity(product);
                articleRepository.Add(articleToAdd);
                
                context.SaveChanges();

                //Act
                articleRepository.Remove(Guid.NewGuid());
                context.SaveChanges();

                //Assert
            }
        }

        [TestMethod]
        public void Remove_Deletes_Product()
        {
            var databaseName = Guid.NewGuid().ToString();
            Guid articleId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var product = ProductFactory.CreateValidEntity();
                var articleToAdd = ArticleFactory.CreateValidEntity(product);
                context.Articles.Add(articleToAdd);
                articleId = articleToAdd.Id;
                context.SaveChanges();
            }

            //Act
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                var articleRepository = new ArticleRepository(context);
                articleRepository.Remove(articleId);
                context.SaveChanges();
            }

            //Assert
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                var articleRepository = new ArticleRepository(context);
                var article = articleRepository.Get(articleId);
                Assert.IsNull(article);
            }
        }
    }
}
