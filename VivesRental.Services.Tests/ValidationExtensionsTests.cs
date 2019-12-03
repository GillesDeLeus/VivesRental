using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Services.Extensions;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
    [TestClass]
    public class ValidationExtensionsTests
    {
        [TestMethod]
        public void Item_IsValid_Returns_True_When_Valid()
        {
            //Arrange
            var item = ItemFactory.CreateValidEntity();

            //Act
            var result = item.IsValid();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Item_IsValid_Returns_False_When_Invalid()
        {
            //Arrange
            var item = ItemFactory.CreateInvalidEntity();

            //Act
            var result = item.IsValid();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Customer_IsValid_Returns_True_When_Valid()
        {
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();

            //Act
            var result = customer.IsValid();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Customer_IsValid_Returns_False_When_Invalid()
        {
            //Arrange
            var customer = CustomerFactory.CreateInvalidEntity();

            //Act
            var result = customer.IsValid();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RentalItem_IsValid_Returns_True_When_Valid()
        {
            //Arrange
            var item = ItemFactory.CreateValidEntity();
            item.Id = Guid.NewGuid();
            var rentalItem = RentalItemFactory.CreateValidEntity(item);
            rentalItem.Id = Guid.NewGuid();

            //Act
            var result = rentalItem.IsValid();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RentalItem_IsValid_Returns_False_When_Invalid()
        {
            //Arrange
            var rentalItem = RentalItemFactory.CreateInvalidEntity();

            //Act
            var result = rentalItem.IsValid();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RentalOrder_IsValid_Returns_True_When_Valid()
        {
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            customer.Id = Guid.NewGuid();
            var rentalOrder = RentalOrderFactory.CreateValidEntity(customer);
            rentalOrder.Id = Guid.NewGuid();

            //Act
            var result = rentalOrder.IsValid();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RentalOrder_IsValid_Returns_False_When_Invalid()
        {
            //Arrange
            var rentalOrder = RentalOrderFactory.CreateInvalidEntity();

            //Act
            var result = rentalOrder.IsValid();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RentalOrderLine_IsValid_Returns_True_When_Valid()
        {
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            customer.Id = Guid.NewGuid();
            var rentalOrder = RentalOrderFactory.CreateValidEntity(customer);
            rentalOrder.Id = Guid.NewGuid();
            var item = ItemFactory.CreateValidEntity();
            item.Id = Guid.NewGuid();
            var rentalItem = RentalItemFactory.CreateValidEntity(item);
            rentalItem.Id = Guid.NewGuid();
            var rentalOrderLine = RentalOrderLineFactory.CreateValidEntity(rentalOrder, rentalItem);
            rentalOrderLine.Id = Guid.NewGuid();

            //Act
            var result = rentalOrderLine.IsValid();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RentalOrderLine_IsValid_Returns_False_When_Invalid()
        {
            //Arrange
            var rentalOrder = RentalOrderFactory.CreateInvalidEntity();

            //Act
            var result = rentalOrder.IsValid();

            //Assert
            Assert.IsFalse(result);
        }
    }
}
