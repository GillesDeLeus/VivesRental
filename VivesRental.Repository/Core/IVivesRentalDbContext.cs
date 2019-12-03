using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;

namespace VivesRental.Repository.Core
{
    public interface IVivesRentalDbContext: IDisposable
    {
        DbSet<Item> Items { get; set; }
        DbSet<RentalItem> RentalItems { get; set; }
        DbSet<Order> RentalOrders { get; set; }
        DbSet<RentalOrderLine> RentalOrderLines { get; set; }
        DbSet<Customer> Customers { get; set; }
        
        //Expose DbContext functionality through interface
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
    }
}