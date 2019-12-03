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
    public class RentalItemRepositoryTests
    {
        
        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_Item()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemRepository = new ItemRepository(context);
                var rentalItemRepository = new RentalItemRepository(context);

                //Act
                var item = ItemFactory.CreateValidEntity();
                itemRepository.Add(item);
                var rentalItem = RentalItemFactory.CreateValidEntity(item);
                rentalItemRepository.Add(rentalItem);

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
                var rentalItemRepository = new RentalItemRepository(context);

                //Act
                var rentalItem = rentalItemRepository.Get(Guid.NewGuid());

                //Assert
                Assert.IsNull(rentalItem);
            }
        }

        [TestMethod]
        public void Get_Returns_Item_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemRepository = new ItemRepository(context);
                var rentalItemRepository = new RentalItemRepository(context);

                var item = ItemFactory.CreateValidEntity();
                itemRepository.Add(item);
                var rentalItemToAdd = RentalItemFactory.CreateValidEntity(item);
                rentalItemRepository.Add(rentalItemToAdd);
                context.SaveChanges();

                //Act
                var rentalItem = rentalItemRepository.Get(rentalItemToAdd.Id);

                //Assert
                Assert.IsNotNull(rentalItem);
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
                var rentalItemRepository = new RentalItemRepository(context);
                for (int i = 0; i < 10; i++)
                {
                    var item = ItemFactory.CreateValidEntity();
                    itemRepository.Add(item);
                    var rentalItemToAdd = RentalItemFactory.CreateValidEntity(item);
                    rentalItemRepository.Add(rentalItemToAdd);
                }
                context.SaveChanges();

                //Act
                var rentalItems = rentalItemRepository.GetAll();

                //Assert
                Assert.AreEqual(10, rentalItems.Count());
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
                var itemRepository = new ItemRepository(context);
                var rentalItemRepository = new RentalItemRepository(context);

                var item = ItemFactory.CreateValidEntity();
                itemRepository.Add(item);
                var rentalItemToAdd = RentalItemFactory.CreateValidEntity(item);
                rentalItemRepository.Add(rentalItemToAdd);
                
                context.SaveChanges();

                //Act
                rentalItemRepository.Remove(Guid.NewGuid());
                context.SaveChanges();

                //Assert
            }
        }

        [TestMethod]
        public void Remove_Deletes_Item()
        {
            var databaseName = Guid.NewGuid().ToString();
            Guid rentalItemId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var item = ItemFactory.CreateValidEntity();
                var rentalItemToAdd = RentalItemFactory.CreateValidEntity(item);
                context.RentalItems.Add(rentalItemToAdd);
                rentalItemId = rentalItemToAdd.Id;
                context.SaveChanges();
            }

            //Act
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                var rentalItemRepository = new RentalItemRepository(context);
                rentalItemRepository.Remove(rentalItemId);
                context.SaveChanges();
            }

            //Assert
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                var rentalItemRepository = new RentalItemRepository(context);
                var rentalItem = rentalItemRepository.Get(rentalItemId);
                Assert.IsNull(rentalItem);
            }
        }
    }
}
