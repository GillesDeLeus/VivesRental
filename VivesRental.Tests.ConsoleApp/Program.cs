using System;
using System.Linq;
using VivesRental.Model;
using VivesRental.Services;
using VivesRental.Tests.ConsoleApp.Factories;

namespace VivesRental.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //TestNumberOfAvailableItems();
            //TestIsAvailable();
            //TestRemove();
            TestEdit2();
            Console.ReadLine();
        }

        static void TestNumberOfAvailableItems()
        {
            using var context = new DbContextFactory().CreateDbContext();
            
            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var articleReservationService = new ArticleReservationService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

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
            using var context = new DbContextFactory().CreateDbContext();
            
            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var articleReservationService = new ArticleReservationService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

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
            using var context = new DbContextFactory().CreateDbContext();
            
            var productService = new ProductService(context);

            var articleService = new ArticleService(context);

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

        static void TestEdit2()
        {
            using var context = new DbContextFactory().CreateDbContext();

            var productService = new ProductService(context);

            var articleService = new ArticleService(context);

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

            var fakeUpdateStatusResult = articleService.UpdateStatus(Guid.NewGuid(), ArticleStatus.Broken);
            product.Id = Guid.NewGuid();
            var fakeUpdateResult = productService.Edit(product);

            productService.Remove(createdProduct.Id);
        }

        static void TestRemove()
        {
            using var context = new DbContextFactory().CreateDbContext();
            
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var customerService = new CustomerService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

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

            var failingDeleteResult = customerService.Remove(Guid.NewGuid());
            var failingDeleteResult2 = productService.Remove(Guid.NewGuid());
        }

        static void TestRemove2()
        {
            using var context = new DbContextFactory().CreateDbContext();
           
            var customerService = new CustomerService(context);

            var deleteResult = customerService.Remove(Guid.Parse("94EF2D02-FD9D-42AE-6461-08D799014437"));
        }
    }
}
