using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VivesRental.Repository.Contracts;

namespace VivesRental.Repository.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IVivesRentalDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IVivesRentalDbContext context,
            IProductRepository productRepository,
            IArticleRepository articleRepository,
            IArticleReservationRepository articleReservationRepository,
            IOrderRepository orderRepository,
            IOrderLineRepository orderLineRepository,
            ICustomerRepository customerRepository)
        {
            _context = context;
            Products = productRepository;
            Articles = articleRepository;
            ArticleReservations = articleReservationRepository;
            Orders = orderRepository;
            OrderLines = orderLineRepository;
            Customers = customerRepository;
        }

        public IProductRepository Products { get; }
        public IArticleRepository Articles { get; }
        public IArticleReservationRepository ArticleReservations { get; }
        public IOrderRepository Orders { get; }
        public IOrderLineRepository OrderLines { get; }
        public ICustomerRepository Customers { get; }

        public int Complete()
        {
            try
            {
                var result = _context.SaveChanges();
                _transaction?.Commit();
                return result;
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
        }

        public void BeginTransaction()
        {
            if (_context.Database.IsInMemory())
            {
                return;
            }
            _transaction = _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

    }
}
