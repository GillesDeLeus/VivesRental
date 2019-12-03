using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class ItemRepositoryTests
    {

        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_Item()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange

                var itemRepository = new ItemRepository(context);
                var item = ItemFactory.CreateValidEntity();

                //Act
                itemRepository.Add(item);
                context.SaveChanges();
            }

            //Assert
            //Use a separate instance of the context to verify correct data was saved to database
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                Assert.AreEqual(1, context.Items.Count());
            }
        }

        [TestMethod]
        public void Get_Returns_Null_When_Not_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemRepository = new ItemRepository(context);

                //Act
                var item = itemRepository.Get(Guid.NewGuid());

                //Assert
                Assert.IsNull(item);
            }
        }

        [TestMethod]
        public void Get_Returns_Item_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            Guid addedItemId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemToAdd = ItemFactory.CreateValidEntity();
                context.Items.Add(itemToAdd);
                context.SaveChanges();
                addedItemId = itemToAdd.Id;
            }

            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Act
                var itemRepository = new ItemRepository(context);
                var item = itemRepository.Get(addedItemId);
                //Assert
                Assert.IsNotNull(item);
            }
        }

        [TestMethod]
        public void GetAll_Returns_10_Items()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemRepository = new ItemRepository(context);
                for (int i = 0; i < 10; i++)
                {
                    var itemToAdd = ItemFactory.CreateValidEntity();
                    itemRepository.Add(itemToAdd);
                }
                context.SaveChanges();

                //Act
                var items = itemRepository.GetAll();

                //Assert
                Assert.AreEqual(10, items.Count());
            }
        }

        [TestMethod]
        public void Remove_Deletes_Item()
        {
            var databaseName = Guid.NewGuid().ToString();
            Guid itemId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemToAdd = ItemFactory.CreateValidEntity();
                context.Items.Add(itemToAdd);
                context.SaveChanges();
                itemId = itemToAdd.Id;
            }

            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Act
                var itemRepository = new ItemRepository(context);

                itemRepository.Remove(itemId);
                context.SaveChanges();

                var item = itemRepository.Get(itemId);

                //Assert
                Assert.IsNull(item);
            }
        }
    }
}
