using VivesRental.Model;

namespace VivesRental.Tests.Data.Factories
{
    public static class RentalItemFactory
    {
        public static RentalItem CreateValidEntity(Item item)
        {
            return new RentalItem
            {
                ItemId = item.Id,
                Item = item,
                Status = RentalItemStatus.InRepair
            };
        }

        public static RentalItem CreateInvalidEntity()
        {
            return new RentalItem
            {
                Status = RentalItemStatus.InRepair
            };
        }
    }
}
