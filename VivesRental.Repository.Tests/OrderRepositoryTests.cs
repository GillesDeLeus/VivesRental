using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class OrderRepositoryTests
    {
       

        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_Order()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);
                var orderRepository = new OrderRepository(context);

                //Act
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var order = OrderFactory.CreateValidEntity(customer);
                orderRepository.Add(order);

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
                var orderRepository = new OrderRepository(context);

                //Act
                var order = orderRepository.Get(Guid.NewGuid());

                //Assert
                Assert.IsNull(order);
            }
        }

        [TestMethod]
        public void Get_Returns_Order_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);
                var orderRepository = new OrderRepository(context);

                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                var orderToAdd = OrderFactory.CreateValidEntity(customer);
                orderRepository.Add(orderToAdd);

                context.SaveChanges();

                //Act
                var order = orderRepository.Get(orderToAdd.Id);

                //Assert
                Assert.IsNotNull(order);
            }
        }

        [TestMethod]
        public void GetAll_Returns_10_Orders()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);
                var orderRepository = new OrderRepository(context);
                for (int i = 0; i < 10; i++)
                {
                    var customer = CustomerFactory.CreateValidEntity();
                    customerRepository.Add(customer);
                    var orderToAdd = OrderFactory.CreateValidEntity(customer);
                    orderRepository.Add(orderToAdd);
                }
                context.SaveChanges();

                //Act
                var orders = orderRepository.GetAll();

                //Assert
                Assert.AreEqual(10, orders.Count());
            }
        }
        
    }
}
