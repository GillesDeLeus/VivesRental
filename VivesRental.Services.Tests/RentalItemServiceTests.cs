using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
	[TestClass]
	public class RentalItemServiceTests
	{
		[TestMethod]
		public void Remove_Deletes_RentalItem()
        {
            //Arrange
            var itemToAdd = ItemFactory.CreateValidEntity();
            var rentalItemToAdd = RentalItemFactory.CreateValidEntity(itemToAdd);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var rentalItemRepositoryMock = new Mock<IRentalItemRepository>();

            //Setup RentalItemRepository
            rentalItemRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<RentalItemIncludes>())).Returns(rentalItemToAdd);
            rentalItemRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            unitOfWorkMock.Setup(uow => uow.RentalItems).Returns(rentalItemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var rentalItemService = new RentalItemService(unitOfWorkMock.Object);

            //Act
            var result = rentalItemService.Remove(rentalItemToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

		[TestMethod]
		public void Remove_Deletes_RentalItem_With_RentalOrderLines()
		{
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            var itemToAdd = ItemFactory.CreateValidEntity();
            var rentalItem = RentalItemFactory.CreateValidEntity(itemToAdd);
            var rentalOrder = RentalOrderFactory.CreateValidEntity(customer);
            var rentalOrderLine = RentalOrderLineFactory.CreateValidEntity(rentalOrder, rentalItem);

            rentalItem.RentalOrderLines.Add(rentalOrderLine);
            itemToAdd.RentalItems.Add(rentalItem);


            //Setup RentalItemRepository
            var rentalItemRepositoryMock = new Mock<IRentalItemRepository>();
            rentalItemRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<RentalItemIncludes>())).Returns(rentalItem);
            rentalItemRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.RentalItems).Returns(rentalItemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var rentalItemService = new RentalItemService(unitOfWorkMock.Object);

            //Act
            var result = rentalItemService.Remove(itemToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }
	}
}
