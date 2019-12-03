using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VivesRental.Repository.Core;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Services.Tests
{
	[TestClass]
	public class RentalItemServiceTests
	{
		[TestMethod]
		public void Remove_Deletes_RentalItem()
        {
            var mock = new Mock<IUnitOfWork>();
			//Arrange
			var itemService = new ItemService(mock.Object);
			var rentalItemService = new RentalItemService(mock.Object);

			var itemToAdd = ItemFactory.CreateValidEntity();
			var newItem = itemService.Create(itemToAdd);

			var rentalItem = RentalItemFactory.CreateValidEntity(newItem);
			var newRentalItem = rentalItemService.Create(rentalItem);

			//Act
			var result = rentalItemService.Remove(newRentalItem.Id);

			//Assert
			Assert.IsTrue(result);

		}

		[TestMethod]
		public void Remove_Deletes_RentalItem_With_RentalOrderLines()
		{
            var mock = new Mock<IUnitOfWork>();
            //Arrange
            var customerService = new CustomerService(mock.Object);
			var itemService = new ItemService(mock.Object);
			var rentalItemService = new RentalItemService(mock.Object);
			var rentalOrderService = new RentalOrderService(mock.Object);
			var rentalOrderLineService = new RentalOrderLineService(mock.Object);

			var customerToAdd = CustomerFactory.CreateValidEntity();
			var customer = customerService.Create(customerToAdd);
			var itemToAdd = ItemFactory.CreateValidEntity();
			var item = itemService.Create(itemToAdd);

			var rentalItemToAdd = RentalItemFactory.CreateValidEntity(item);
			var rentalItem = rentalItemService.Create(rentalItemToAdd);

			var rentalOrder = rentalOrderService.Create(customer.Id);

			//var rentalOrderLineToAdd = RentalOrderLineFactory.CreateValidEntity(rentalOrder, rentalItem);
			var rentalOrderLine = rentalOrderLineService.Rent(rentalOrder.Id, rentalItem.Id);

			//Act
			var result = rentalItemService.Remove(rentalItem.Id);

			//Assert
			Assert.IsTrue(result);

		}
	}
}
