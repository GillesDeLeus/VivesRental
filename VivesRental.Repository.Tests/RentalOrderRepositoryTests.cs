using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class RentalOrderRepositoryTests
    {
       

        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_RentalOrder()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);
                var rentalOrderRepository = new RentalOrderRepository(context);

                //Act
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var rentalOrder = RentalOrderFactory.CreateValidEntity(customer);
                rentalOrderRepository.Add(rentalOrder);

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
                var customerRepository = new CustomerRepository(context);
                var rentalOrderRepository = new RentalOrderRepository(context);

                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var rentalOrderToAdd = RentalOrderFactory.CreateValidEntity(customer);
                rentalOrderRepository.Add(rentalOrderToAdd);

                context.SaveChanges();

                //Act
                var rentalOrder = rentalOrderRepository.Get(rentalOrderToAdd.Id);

                //Assert
                Assert.IsNotNull(rentalOrder);
            }
        }

        [TestMethod]
        public void GetAll_Returns_10_RentalOrders()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);
                var rentalOrderRepository = new RentalOrderRepository(context);
                for (int i = 0; i < 10; i++)
                {
                    var customer = CustomerFactory.CreateValidEntity();
                    customerRepository.Add(customer);
                    var rentalOrderToAdd = RentalOrderFactory.CreateValidEntity(customer);
                    rentalOrderRepository.Add(rentalOrderToAdd);
                }
                context.SaveChanges();

                //Act
                var rentalOrders = rentalOrderRepository.GetAll();

                //Assert
                Assert.AreEqual(10, rentalOrders.Count());
            }
        }
        
    }
}
