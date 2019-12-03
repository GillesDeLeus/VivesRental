using System;
using VivesRental.Model;

namespace VivesRental.Services.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsValid(this Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                return false;

            return true;
        }

        public static bool IsValid(this RentalItem rentalItem)
        {
            if (rentalItem.ItemId == Guid.Empty)
                return false;
            
            return true;
        }

        public static bool IsValid(this Order rentalOrder)
        {
            if (rentalOrder.CustomerId == Guid.Empty)
                return false;

            if (string.IsNullOrWhiteSpace(rentalOrder.CustomerFirstName))
                return false;

            if (string.IsNullOrWhiteSpace(rentalOrder.CustomerLastName))
                return false;

            if (string.IsNullOrWhiteSpace(rentalOrder.CustomerEmail))
                return false;

            if (rentalOrder.CreatedAt == DateTime.MinValue)
                return false;

            return true;
        }

        public static bool IsValid(this RentalOrderLine rentalOrderLine)
        {
            if (rentalOrderLine.RentalOrderId == Guid.Empty)
                return false;

            if (rentalOrderLine.RentalItemId == Guid.Empty)
                return false;

            if (string.IsNullOrWhiteSpace(rentalOrderLine.ItemName))
                return false;

            if (rentalOrderLine.RentedAt == DateTime.MinValue)
                return false;

            if (rentalOrderLine.ExpiresAt == DateTime.MinValue)
                return false;

            return true;
        }

        public static bool IsValid(this Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(customer.Name))
                return false;

            if (string.IsNullOrWhiteSpace(customer.Email))
                return false;

            return true;
        }
    }
}
