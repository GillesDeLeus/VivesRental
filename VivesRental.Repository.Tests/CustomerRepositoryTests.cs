using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class CustomerRepositoryTests
    {


        [TestMethod]
        public void Add_Returns_1_When_Adding_Valid_Customer()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);

                //Act
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);

                var result = context.SaveChanges();

                //Assert
                Assert.AreEqual(1, result);
            }
        }

        [TestMethod]
        public void Get_Returns_Null_When_Not_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);

                //Act
                var customer = customerRepository.Get(Guid.NewGuid());

                //Assert
                Assert.IsNull(customer);
            }
        }

        [TestMethod]
        public void Get_Returns_Customer_When_Found()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);
                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);
                context.SaveChanges();

                //Act
                var dbCustomer = customerRepository.Get(customer.Id);

                //Assert
                Assert.IsNotNull(dbCustomer);
            }
        }

        [TestMethod]
        public void GetAll_Returns_10_Customers()
        {
            var databaseName = Guid.NewGuid().ToString();
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                //Arrange
                var customerRepository = new CustomerRepository(context);
                for (int i = 0; i < 10; i++)
                {
                    var customer = CustomerFactory.CreateValidEntity();
                    customerRepository.Add(customer);
                }
                context.SaveChanges();

                //Act
                var customers = customerRepository.GetAll();

                //Assert
                Assert.AreEqual(10, customers.Count());
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
                var customerRepository = new CustomerRepository(context);

                var customer = CustomerFactory.CreateValidEntity();
                customerRepository.Add(customer);

                context.SaveChanges();

                //Act
                customerRepository.Remove(Guid.NewGuid());
                context.SaveChanges();

                //Assert
            }
        }

        [TestMethod]
        public void Remove_Deletes_Customer()
        {
            //Arrange
            var databaseName = Guid.NewGuid().ToString();
            Guid customerId;
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                var customer = CustomerFactory.CreateValidEntity();
                context.Customers.Add(customer);
                customerId = customer.Id;
                context.SaveChanges();
            }

            //Act
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                var customerRepository = new CustomerRepository(context);
                customerRepository.Remove(customerId);
                context.SaveChanges();
            }

            //Assert
            using (var context = DbContextFactory.CreateInstance(databaseName))
            {
                var dbCustomer = context.Customers.Find(customerId);

                Assert.IsNull(dbCustomer);
            }
        }
    }
}
