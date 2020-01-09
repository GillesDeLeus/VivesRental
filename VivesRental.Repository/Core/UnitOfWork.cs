using VivesRental.Repository.Contracts;

namespace VivesRental.Repository.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IVivesRentalDbContext _context;

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
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
