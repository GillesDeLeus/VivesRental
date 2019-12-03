using VivesRental.Repository.Contracts;

namespace VivesRental.Repository.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IVivesRentalDbContext _context;

        public UnitOfWork(IVivesRentalDbContext context,
            IItemRepository itemRepository,
            IRentalItemRepository rentalItemRepository,
            IRentalOrderRepository rentalOrderRepository,
            IRentalOrderLineRepository rentalOrderLineRepository,
            ICustomerRepository customerRepository)
        {
            _context = context;
            Items = itemRepository;
            RentalItems = rentalItemRepository;
            RentalOrders = rentalOrderRepository;
            RentalOrderLines = rentalOrderLineRepository;
            Customers = customerRepository;
        }

        public IItemRepository Items { get; }
        public IRentalItemRepository RentalItems { get; }
        public IRentalOrderRepository RentalOrders { get; }
        public IRentalOrderLineRepository RentalOrderLines { get; }
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
