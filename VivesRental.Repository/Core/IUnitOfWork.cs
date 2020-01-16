using System;
using VivesRental.Repository.Contracts;

namespace VivesRental.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IArticleRepository Articles { get; }
        IArticleReservationRepository ArticleReservations { get; }
        IOrderRepository Orders { get; }
        IOrderLineRepository OrderLines { get; }
        ICustomerRepository Customers { get; }
        int Complete();
        void BeginTransaction();
    }
}
