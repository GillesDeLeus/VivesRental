using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
    [TestClass]
    public class ItemServiceTests
    {
        [TestMethod]
        public void Remove_Deletes_Item()
        {
            //Arrange
            var itemToAdd = ItemFactory.CreateValidEntity();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var itemRepositoryMock = new Mock<IItemRepository>();
            
            //Setup ItemRepository
            itemRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ItemIncludes>())).Returns(itemToAdd);
            itemRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            unitOfWorkMock.Setup(uow => uow.Items).Returns(itemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);
            
            var itemService = new ItemService(unitOfWorkMock.Object);

            //Act
            var result = itemService.Remove(itemToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_Returns_False_When_Item_Is_Null()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var itemRepositoryMock = new Mock<IItemRepository>();

            //Setup ItemRepository
            itemRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ItemIncludes>())).Returns((Item)null);
            itemRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            unitOfWorkMock.Setup(uow => uow.Items).Returns(itemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var itemService = new ItemService(unitOfWorkMock.Object);

            //Act
            var result = itemService.Remove(Guid.NewGuid());

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Remove_Deletes_Item_With_RentalItems()
        {
            //Arrange
            var itemToAdd = ItemFactory.CreateValidEntity();
            var rentalItem = RentalItemFactory.CreateValidEntity(itemToAdd);
            itemToAdd.RentalItems.Add(rentalItem);


            //Setup ItemRepository
            var itemRepositoryMock = new Mock<IItemRepository>();
            itemRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ItemIncludes>())).Returns(itemToAdd);
            itemRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup RentalItemRepository
            var rentalItemRepositoryMock = new Mock<IRentalItemRepository>();
            rentalItemRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Items).Returns(itemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.RentalItems).Returns(rentalItemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var itemService = new ItemService(unitOfWorkMock.Object);

            //Act
            var result = itemService.Remove(itemToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_Deletes_Item_With_RentalItems_And_RentalOrderLines()
        {
            //Arrange
            var customer = CustomerFactory.CreateValidEntity();
            var itemToAdd = ItemFactory.CreateValidEntity();
            var rentalItem = RentalItemFactory.CreateValidEntity(itemToAdd);
            var rentalOrder = RentalOrderFactory.CreateValidEntity(customer);
            var rentalOrderLine = RentalOrderLineFactory.CreateValidEntity(rentalOrder, rentalItem);

            rentalItem.RentalOrderLines.Add(rentalOrderLine);
            itemToAdd.RentalItems.Add(rentalItem);


            //Setup ItemRepository
            var itemRepositoryMock = new Mock<IItemRepository>();
            itemRepositoryMock.Setup(ir => ir.Get(It.IsAny<Guid>(), It.IsAny<ItemIncludes>())).Returns(itemToAdd);
            itemRepositoryMock.Setup(ir => ir.Remove(It.IsAny<Guid>()));

            //Setup RentalItemRepository
            var rentalItemRepositoryMock = new Mock<IRentalItemRepository>();
            rentalItemRepositoryMock.Setup(rir => rir.Remove(It.IsAny<Guid>()));

            //Setup UnitOfWork
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Items).Returns(itemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.RentalItems).Returns(rentalItemRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Returns(1);

            var itemService = new ItemService(unitOfWorkMock.Object);

            //Act
            var result = itemService.Remove(itemToAdd.Id);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
