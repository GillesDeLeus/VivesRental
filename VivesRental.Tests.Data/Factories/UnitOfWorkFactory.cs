using VivesRental.Repository;
using VivesRental.Repository.Core;

namespace VivesRental.Tests.Data.Factories
{
    public static class UnitOfWorkFactory
    {
        public static IUnitOfWork CreateInstance(IVivesRentalDbContext context)
        {
            var productRepository = new ProductRepository(context);
            var articleRepository = new ArticleRepository(context);
            var customerRepository = new CustomerRepository(context);
            var orderRepository = new OrderRepository(context);
            var orderLineRepository = new OrderLineRepository(context);

            //Setup UnitOfWork
            return new UnitOfWork(context, productRepository, articleRepository, orderRepository, orderLineRepository, customerRepository);
        }
    }
}
