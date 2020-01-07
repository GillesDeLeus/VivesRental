using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
    [TestClass]
    public class CustomerServiceTests
    {
        [TestMethod]
        public void Remove_Deletes_Customer()
        {
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customerService = new CustomerService(unitOfWorkMock.Object);

            customerRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>())).Returns(customer);
            customerRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));
            unitOfWorkMock.Setup(uow => uow.Customers).Returns(customerRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            //Act
            var result = customerService.Remove(customer.Id);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
