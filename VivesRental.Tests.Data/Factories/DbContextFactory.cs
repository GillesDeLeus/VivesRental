using Microsoft.EntityFrameworkCore;
using VivesRental.Repository.Core;

namespace VivesRental.Tests.Data.Factories
{
    public static class DbContextFactory
    {
        public static IVivesRentalDbContext CreateInstance(string databaseName)
        {
            var options = new DbContextOptionsBuilder<VivesRentalDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new VivesRentalDbContext(options);
        }
    }
}
