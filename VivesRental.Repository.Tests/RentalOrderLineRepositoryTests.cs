using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class RentalOrderLineRepositoryTests
    {
        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_RentalOrder()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemRepository = new ItemRepository(context);
                var rentalItemRepository = new RentalItemRepository(context);
                var customerRepository = new CustomerRepository(context);
                var rentalOrderRepository = new RentalOrderRepository(context);
                var rentalOrderLineRepository = new RentalOrderLineRepository(context);

                //Act
                var item = ItemFactory.CreateValidEntity();
                itemRepository.Add(item);
                var rentalItem = RentalItemFactory.CreateValidEntity(item);
                rentalItemRepository.Add(rentalItem);
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var rentalOrder = RentalOrderFactory.CreateValidEntity(customer);
                rentalOrderRepository.Add(rentalOrder);
                var rentalOrderLine = RentalOrderLineFactory.CreateValidEntity(rentalOrder, rentalItem);
                rentalOrderLineRepository.Add(rentalOrderLine);

                var result = context.SaveChanges();

                //Assert
                Assert.AreEqual(5, result); //Because we added five entities
            }
        }

        [TestMethod]
        public void Get_Returns_Null_When_Not_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var rentalOrderRepository = new RentalOrderRepository(context);

                //Act
                var rentalOrder = rentalOrderRepository.Get(Guid.NewGuid());

                //Assert
                Assert.IsNull(rentalOrder);
            }
        }

        [TestMethod]
        public void Get_Returns_RentalOrder_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var itemRepository = new ItemRepository(context);
                var rentalItemRepository = new RentalItemRepository(context);
                var customerRepository = new CustomerRepository(context);
                var rentalOrderRepository = new RentalOrderRepository(context);
                var rentalOrderLineRepository = new RentalOrderLineRepository(context);

                var item = ItemFactory.CreateValidEntity();
                itemRepository.Add(item);
                var rentalItem = RentalItemFactory.CreateValidEntity(item);
                rentalItemRepository.Add(rentalItem);
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var rentalOrder = RentalOrderFactory.CreateValidEntity(customer);
                rentalOrderRepository.Add(rentalOrder);
                var rentalOrderLineToAdd = RentalOrderLineFactory.CreateValidEntity(rentalOrder, rentalItem);
                rentalOrderLineRepository.Add(rentalOrderLineToAdd);

                context.SaveChanges();

                //Act
                var rentalOrderLine = rentalOrderLineRepository.Get(rentalOrderLineToAdd.Id);

                //Assert
                Assert.IsNotNull(rentalOrderLine);
            }
        }
    }
}
