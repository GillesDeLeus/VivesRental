using System;
using VivesRental.Model;

namespace VivesRental.Tests.Data.Factories
{
    public static class OrderFactory
    {
        public static Order CreateValidEntity(Customer customer)
        {
            return new Order
            {
                CustomerId = customer.Id,
                Customer = customer,
                CustomerFirstName = "TestFirstName",
                CustomerLastName = "TestLastName",
                CustomerEmail = "TestEmail",
                CreatedAt = DateTime.Now
            };
        }

        public static Order CreateInvalidEntity()
        {
            return new Order
            {
                CreatedAt = DateTime.Now
            };
        }
    }
}
