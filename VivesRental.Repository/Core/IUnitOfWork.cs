using System;
using VivesRental.Repository.Contracts;

namespace VivesRental.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository Items { get; }
        IRentalItemRepository RentalItems { get; }
        IRentalOrderRepository RentalOrders { get; }
        IRentalOrderLineRepository RentalOrderLines { get; }
        ICustomerRepository Customers { get; }
        int Complete();
    }
}
