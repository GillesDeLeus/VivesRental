using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Repository.Includes;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class ProductRepositoryTests
    {

        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_Product()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange

                var productRepository = new ProductRepository(context);
                var product = ProductFactory.CreateValidEntity();

                //Act
                productRepository.Add(product);
                context.SaveChanges();
            }

            //Assert
            //Use a separate instance of the context to verify correct data was saved to database
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                Assert.AreEqual(1, context.Products.Count());
            }
        }

        [TestMethod]
        public void Get_Returns_Null_When_Not_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using var context = DbContextFactory.CreateInstance(databaseName);

            //Arrange
            var productRepository = new ProductRepository(context);

            //Act
            var product = productRepository.Get(Guid.NewGuid());

            //Assert
            Assert.IsNull(product);
        }

        [TestMethod]
        public void Get_Returns_Null_When_Not_Found_With_Includes()
        {
            var databaseName = Guid.NewGuid().ToString();
            using var context = DbContextFactory.CreateInstance(databaseName);

            //Arrange
            var productRepository = new ProductRepository(context);

            //Act
            var product = productRepository.Get(Guid.NewGuid(), new ProductIncludes{ArticleOrderLines = true});

            //Assert
            Assert.IsNull(product);
        }

        [TestMethod]
        public void Get_Returns_Product_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            Guid addedProductId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productToAdd = ProductFactory.CreateValidEntity();
                context.Products.Add(productToAdd);
                context.SaveChanges();
                addedProductId = productToAdd.Id;
            }

            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Act
                var productRepository = new ProductRepository(context);
                var product = productRepository.Get(addedProductId);
                //Assert
                Assert.IsNotNull(product);
            }
        }

        [TestMethod]
        public void Get_Returns_Product_When_Found_With_Includes()
        {
            var databaseName = Guid.NewGuid().ToString();
            Guid addedProductId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productToAdd = ProductFactory.CreateValidEntity();
                context.Products.Add(productToAdd);
                context.SaveChanges();
                addedProductId = productToAdd.Id;
            }

            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Act
                var productRepository = new ProductRepository(context);
                var product = productRepository.Get(addedProductId, new ProductIncludes { ArticleOrderLines = true });
                //Assert
                Assert.IsNotNull(product);
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
                for (int i = 0; i < 10; i++)
                {
                    var productToAdd = ProductFactory.CreateValidEntity();
                    productRepository.Add(productToAdd);
                }
                context.SaveChanges();

                //Act
                var products = productRepository.GetAll();

                //Assert
                Assert.AreEqual(10, products.Count());
            }
        }

        [TestMethod]
        public void Remove_Deletes_Product()
        {
            var databaseName = Guid.NewGuid().ToString();
            Guid productId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var productToAdd = ProductFactory.CreateValidEntity();
                context.Products.Add(productToAdd);
                context.SaveChanges();
                productId = productToAdd.Id;
            }

            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Act
                var productRepository = new ProductRepository(context);

                productRepository.Remove(productId);
                context.SaveChanges();

                var product = productRepository.Get(productId);

                //Assert
                Assert.IsNull(product);
            }
        }
    }
}
