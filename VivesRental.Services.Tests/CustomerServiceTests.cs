using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            ////Arrange
            //var customer = CustomerFactory.CreateValidEntity();
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var customerRepositoryMock = new Mock<ICustomerRepository>();
            //var orderRepositoryMock = new Mock<IOrderRepository>();
            //var customerService = new CustomerService(unitOfWorkMock.Object);

            //customerRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));
            //unitOfWorkMock.Setup(uow => uow.Customers).Returns(customerRepositoryMock.Object);
            //unitOfWorkMock.Setup(uow => uow.Orders).Returns(orderRepositoryMock.Object);
            //unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            ////Act
            //var result = customerService.Remove(customer.Id);

            ////Assert
            //Assert.IsTrue(result);
        }

    }
}
