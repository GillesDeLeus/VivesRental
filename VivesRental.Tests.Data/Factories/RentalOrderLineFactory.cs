using System;
using VivesRental.Model;

namespace VivesRental.Tests.Data.Factories
{
    public static class RentalOrderLineFactory
    {
        public static RentalOrderLine CreateValidEntity(Order rentalOrder, RentalItem rentalItem)
        {
            return new RentalOrderLine
            {
                RentalOrderId =  rentalOrder.Id,
                RentalOrder = rentalOrder,
                RentalItemId = rentalItem.Id,
                RentalItem = rentalItem,
                ItemName = "TestItemName",
                ItemDescription = "TestItemDescription",
                RentedAt = DateTime.Now,
                ExpiresAt = DateTime.Now,
                ReturnedAt = DateTime.Now
            };
        }

        public static RentalOrderLine CreateInvalidEntity()
        {
            return new RentalOrderLine
            {
                RentedAt = DateTime.Now
            };
        }
    }
}
