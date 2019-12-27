using Microsoft.EntityFrameworkCore;
using VivesRental.Model;

namespace VivesRental.Repository.Core
{
    public class VivesRentalDbContext: DbContext, IVivesRentalDbContext
    {
        public VivesRentalDbContext()
        {
            
        }

        public VivesRentalDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }

    
}
