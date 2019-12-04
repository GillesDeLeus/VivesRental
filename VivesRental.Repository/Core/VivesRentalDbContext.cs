using Microsoft.EntityFrameworkCore;
using VivesRental.Model;

namespace VivesRental.Repository.Core
{
    public class VivesRentalDbContext: DbContext, IVivesRentalDbContext
    {
        public VivesRentalDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<RentalItem> RentalItems { get; set; }
        public DbSet<Order> RentalOrders { get; set; }
        public DbSet<RentalOrderLine> RentalOrderLines { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

    
}
