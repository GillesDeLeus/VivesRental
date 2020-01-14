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
            using var context = new DbContextFactory().CreateDbContext();
            var unitOfWork = CreateUnitOfWork(context);

            var productService = new ProductService(unitOfWork);
            var articleService = new ArticleService(unitOfWork);
            var customerService = new CustomerService(unitOfWork);
            var orderService = new OrderService(unitOfWork);
            var orderLineService = new OrderLineService(unitOfWork);

            var customer = customerService.Create(new Customer
                {FirstName = "Test", LastName = "Test", Email = "test@test.com"});
            var product = productService.Create(new Product
            {
                Name = "Test",
                Description = "Test",
                Manufacturer = "Test",
                Publisher = "Test",
                RentalExpiresAfterDays = 10
            });
           
            var article = articleService.Create(new Article
            {
                ProductId = product.Id,
                Status = ArticleStatus.Normal
            });
            var order = orderService.Create(customer.Id);
            var orderLine = orderLineService.Rent(order.Id, article.Id);


            var deleteResult = customerService.Remove(product.Id);
        }

        static void TestRemove2()
        {
            using var context = new DbContextFactory().CreateDbContext();
            var unitOfWork = CreateUnitOfWork(context);

            var customerService = new CustomerService(unitOfWork);

            var deleteResult = customerService.Remove(Guid.Parse("94EF2D02-FD9D-42AE-6461-08D799014437"));
        }

        static IUnitOfWork CreateUnitOfWork(IVivesRentalDbContext context)
        {
            
            var productRepository = new ProductRepository(context);
            var articleRepository = new ArticleRepository(context);
            var articleReservationRepository = new ArticleReservationRepository(dbContext);
            var orderRepository = new OrderRepository(context);
            var orderLineRepository = new OrderLineRepository(context);
            var customerRepository = new CustomerRepository(context);
            return new UnitOfWork(context, productRepository, articleRepository, orderRepository, orderLineRepository, customerRepository);
        }
    }
}
