using VivesRental.Model;

namespace VivesRental.Tests.Data.Factories
{
    public static class ItemFactory
    {
        public static Item CreateValidEntity()
        {
            return new Item
            {
                Name = "TestName",
                Description = "TestDescription",
                Manufacturer = "TestManufacturer",
                Publisher = "TestPublisher",
                RentalExpiresAfterDays = 10
            };
        }

        public static Item CreateInvalidEntity()
        {
            return new Item
            {
                Description = "TestDescription",
                Manufacturer = "TestManufacturer",
                Publisher = "TestPublisher"
            };
        }
    }
}
