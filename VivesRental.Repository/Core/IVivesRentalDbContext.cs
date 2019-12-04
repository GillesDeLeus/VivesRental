using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;

namespace VivesRental.Repository.Core
{
    public interface IVivesRentalDbContext: IDisposable
    {
        DbSet<Product> Products { get; set; }
        DbSet<Article> Articles { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderLine> OrderLines { get; set; }
        DbSet<Customer> Customers { get; set; }
        
        //Expose DbContext functionality through interface
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
       
    }
}