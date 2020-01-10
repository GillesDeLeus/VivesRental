using System;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository;
using VivesRental.Repository.Core;
using VivesRental.Services;
using VivesRental.Tests.ConsoleApp.Factories;

namespace VivesRental.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestNumberOfAvailableItems();
            //TestIsAvailable();
            Console.ReadLine();
        }

        static void TestNumberOfAvailableItems()
        {
            using var dbContext = new DbContextFactory().CreateDbContext();
            var unitOfWork = CreateUnitOfWork(dbContext);

            var customerService = new CustomerService(unitOfWork);
            var productService = new ProductService(unitOfWork);
            var articleService = new ArticleService(unitOfWork);
            var articleReservationService = new ArticleReservationService(unitOfWork);
            var orderService = new OrderService(unitOfWork);
            var orderLineService = new OrderLineService(unitOfWork, articleService);

            //Create Customer
            var customer = customerService.Create(new Customer { FirstName = "Bavo", LastName = "Ketels", Email = "bavo.ketels@vives.be", PhoneNumber = "test" });

            //Create Product
            var product = productService.Create(new Product { Name = "Product", RentalExpiresAfterDays = 1 });

            //Create Article
            var article = articleService.Create(new Article { ProductId = product.Id });
            var productResults = productService.GetAvailableProductResults();
            Console.WriteLine($"availableProducts (1): {productResults.First().NumberOfAvailableArticles}");

            //Edit Article
            articleService.UpdateStatus(article.Id, ArticleStatus.Broken);
            var productResultsBroken = productService.GetAvailableProductResults();
            Console.WriteLine($"availableProducts Broken (0): {productResultsBroken.Count}");

            //Add OrderLine
            articleService.UpdateStatus(article.Id, ArticleStatus.Normal);
            var order = orderService.Create(customer.Id);
            orderLineService.Rent(order.Id, article.Id);
            var productResultsRented = productService.GetAvailableProductResults();
            Console.WriteLine($"availableProducts Rented (0): {productResultsRented.Count}");

            //Return Order
            orderService.Return(order.Id, DateTime.Now);
            var productResultsReturned = productService.GetAvailableProductResults();
            Console.WriteLine($"availableProducts Returned (1): {productResultsReturned.First().NumberOfAvailableArticles}");

            //Create ArticleReservation
            var articleReservation = articleReservationService.Create(customer.Id, article.Id);
            var productResultsReserved = productService.GetAvailableProductResults();
            Console.WriteLine($"availableProducts Reserved (0): {productResultsReserved.Count}");
        }

        static void TestIsAvailable()
        {
            using var dbContext = new DbContextFactory().CreateDbContext();
            var unitOfWork = CreateUnitOfWork(dbContext);

            var customerService = new CustomerService(unitOfWork);
            var productService = new ProductService(unitOfWork);
            var articleService = new ArticleService(unitOfWork);
            var articleReservationService = new ArticleReservationService(unitOfWork);
            var orderService = new OrderService(unitOfWork);
            var orderLineService = new OrderLineService(unitOfWork, articleService);

            //Create Customer
            var customer = customerService.Create(new Customer { FirstName = "Bavo", LastName = "Ketels", Email = "bavo.ketels@vives.be", PhoneNumber = "test" });

            //Create Product
            var product = productService.Create(new Product { Name = "Product", RentalExpiresAfterDays = 1 });

            //Create Article
            var article = articleService.Create(new Article { ProductId = product.Id });
            var isAvailableWithoutReservations = articleService.IsAvailable(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableWithoutReservations (true): {isAvailableWithoutReservations}");

            //Edit Article
            articleService.UpdateStatus(article.Id, ArticleStatus.Broken);
            var isAvailableBroken = articleService.IsAvailable(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableBroken (false): {isAvailableBroken}");

            //Add OrderLine
            articleService.UpdateStatus(article.Id, ArticleStatus.Normal);
            var order = orderService.Create(customer.Id);
            orderLineService.Rent(order.Id, article.Id);
            var isAvailableRented = articleService.IsAvailable(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableRented (false): {isAvailableRented}");

            //Return Order
            orderService.Return(order.Id, DateTime.Now);
            var isAvailableReturned = articleService.IsAvailable(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableReturned (true): {isAvailableReturned}");

            //Create ArticleReservation
            var articleReservation = articleReservationService.Create(customer.Id, article.Id);
            var isAvailableWithReservations = articleService.IsAvailable(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableWithReservations (false): {isAvailableWithReservations}");
        }

        static void TestEdit()
        {
            using var dbContext = new DbContextFactory().CreateDbContext();
            var unitOfWork = CreateUnitOfWork(dbContext);

            var productService = new ProductService(unitOfWork);

            var articleService = new ArticleService(unitOfWork);

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Manufacturer = "Test",
                Publisher = "Test",
                RentalExpiresAfterDays = 10
            };
            var createdProduct = productService.Create(product);
            var article = new Article
            {
                ProductId = createdProduct.Id,
                Status = ArticleStatus.Normal
            };
            var createdArticle = articleService.Create(article);

            var updateStatusResult = articleService.UpdateStatus(createdArticle.Id, ArticleStatus.Broken);

            productService.Remove(createdProduct.Id);
        }

        static void TestRemove()
        {

        }

        private static IUnitOfWork CreateUnitOfWork(VivesRentalDbContext dbContext)
        {
            var productRepository = new ProductRepository(dbContext);
            var articleRepository = new ArticleRepository(dbContext);
            var articleReservationRepository = new ArticleReservationRepository(dbContext);
            var orderRepository = new OrderRepository(dbContext);
            var orderLineRepository = new OrderLineRepository(dbContext);
            var customerRepository = new CustomerRepository(dbContext);

            return new UnitOfWork(dbContext, productRepository, articleRepository, articleReservationRepository, orderRepository, orderLineRepository, customerRepository);
        }
    }
}
