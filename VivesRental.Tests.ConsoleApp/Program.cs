using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Services;
using VivesRental.Tests.ConsoleApp.Factories;

namespace VivesRental.Tests.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //await TestNumberOfAvailableItems();
            //TestIsAvailable();
            //TestRemove();
            //await TestEdit2();
            //await GetOrder(new Guid("EAAC2797-C0FE-4FE2-029C-08D79A9866A1"));
            await RentArticles(new Guid("EAAC2797-C0FE-4FE2-029C-08D79A9866A1"),
                new Guid("1FBAC675-CE6D-4BAA-09C7-08D79A986674"));
            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        static async Task TestNumberOfAvailableItems()
        {
            await using var context = new DbContextFactory().CreateDbContext();
            
            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var articleReservationService = new ArticleReservationService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

            //Create Customer
            var customer = await customerService.CreateAsync(new Customer { FirstName = "Bavo", LastName = "Ketels", Email = "bavo.ketels@vives.be", PhoneNumber = "test" });

            //Create Product
            var product = await productService.CreateAsync(new Product { Name = "Product", RentalExpiresAfterDays = 1 });

            //Create Article
            var article = await articleService.CreateAsync(new Article { ProductId = product.Id });
            var productResults = await productService.GetAvailableProductResultsAsync();
            Console.WriteLine($"availableProducts (1): {productResults.First().NumberOfAvailableArticles}");

            //Edit Article
            await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Broken);
            var productResultsBroken = await productService.GetAvailableProductResultsAsync();
            Console.WriteLine($"availableProducts Broken (0): {productResultsBroken.Count}");

            //Add OrderLine
            await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Normal);
            var order = await orderService.CreateAsync(customer.Id);
            await orderLineService.RentAsync(order.Id, article.Id);
            var productResultsRented = await productService.GetAvailableProductResultsAsync();
            Console.WriteLine($"availableProducts Rented (0): {productResultsRented.Count}");

            //Return Order
            await orderService.ReturnAsync(order.Id, DateTime.Now);
            var productResultsReturned = await productService.GetAvailableProductResultsAsync();
            Console.WriteLine($"availableProducts Returned (1): {productResultsReturned.First().NumberOfAvailableArticles}");

            //Create ArticleReservation
            var articleReservation = await articleReservationService.CreateAsync(customer.Id, article.Id);
            var productResultsReserved = await productService.GetAvailableProductResultsAsync();
            Console.WriteLine($"availableProducts Reserved (0): {productResultsReserved.Count}");
        }

        static async Task TestIsAvailable()
        {
            await using var context = new DbContextFactory().CreateDbContext();
            
            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var articleReservationService = new ArticleReservationService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

            //Create Customer
            var customer = await customerService.CreateAsync(new Customer { FirstName = "Bavo", LastName = "Ketels", Email = "bavo.ketels@vives.be", PhoneNumber = "test" });

            //Create Product
            var product = await productService.CreateAsync(new Product { Name = "Product", RentalExpiresAfterDays = 1 });

            //Create Article
            var article = await articleService.CreateAsync(new Article { ProductId = product.Id });
            var isAvailableWithoutReservations = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableWithoutReservations (true): {isAvailableWithoutReservations}");

            //Edit Article
            await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Broken);
            var isAvailableBroken = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableBroken (false): {isAvailableBroken}");

            //Add OrderLine
            await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Normal);
            var order = await orderService.CreateAsync(customer.Id);
            await orderLineService.RentAsync(order.Id, article.Id);
            var isAvailableRented = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableRented (false): {isAvailableRented}");

            //Return Order
            await orderService.ReturnAsync(order.Id, DateTime.Now);
            var isAvailableReturned = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableReturned (true): {isAvailableReturned}");

            //Create ArticleReservation
            var articleReservation = await articleReservationService.CreateAsync(customer.Id, article.Id);
            var isAvailableWithReservations = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableWithReservations (false): {isAvailableWithReservations}");
        }

        static async Task TestEdit()
        {
            await using var context = new DbContextFactory().CreateDbContext();
            
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
            var createdProduct = await productService.CreateAsync(product);
            var article = new Article
            {
                ProductId = createdProduct.Id,
                Status = ArticleStatus.Normal
            };
            var createdArticle = await articleService.CreateAsync(article);

            var updateStatusResult = await articleService.UpdateStatusAsync(createdArticle.Id, ArticleStatus.Broken);

            await productService.RemoveAsync(createdProduct.Id);
        }

        static async Task TestEdit2()
        {
            await using var context = new DbContextFactory().CreateDbContext();

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
            var createdProduct = await productService.CreateAsync(product);
            var article = new Article
            {
                ProductId = createdProduct.Id,
                Status = ArticleStatus.Normal
            };
            var createdArticle = await articleService.CreateAsync(article);

            var fakeUpdateStatusResult = await articleService.UpdateStatusAsync(Guid.NewGuid(), ArticleStatus.Broken);
            product.Id = Guid.NewGuid();
            var fakeUpdateResult = await productService.EditAsync(product);

            await productService.RemoveAsync(createdProduct.Id);
        }

        static async Task TestRemove()
        {
            using var context = new DbContextFactory().CreateDbContext();
            
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var customerService = new CustomerService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

            var customer = await customerService.CreateAsync(new Customer
                {FirstName = "Test", LastName = "Test", Email = "test@test.com"});
            var product = await productService.CreateAsync(new Product
            {
                Name = "Test",
                Description = "Test",
                Manufacturer = "Test",
                Publisher = "Test",
                RentalExpiresAfterDays = 10
            });
           
            var article = await articleService.CreateAsync(new Article
            {
                ProductId = product.Id,
                Status = ArticleStatus.Normal
            });
            var order = await orderService.CreateAsync(customer.Id);
            var orderLine = await orderLineService.RentAsync(order.Id, article.Id);


            var deleteResult = await customerService.RemoveAsync(product.Id);

            var failingDeleteResult = await customerService.RemoveAsync(Guid.NewGuid());
            var failingDeleteResult2 = await productService.RemoveAsync(Guid.NewGuid());
        }

        static async Task TestRemove2()
        {
            await using var context = new DbContextFactory().CreateDbContext();
           
            var customerService = new CustomerService(context);

            var deleteResult = await customerService.RemoveAsync(Guid.Parse("94EF2D02-FD9D-42AE-6461-08D799014437"));
        }

        static async Task GetOrder(Guid orderId)
        {
            await using var context = new DbContextFactory().CreateDbContext();
            var orderService = new OrderService(context);
            var orderResult = await orderService.GetAsync(orderId);

            Console.WriteLine($"orderResult.NumberOfOrderLines (1): {orderResult.NumberOfOrderLines}");

        }

        static async Task RentArticles(Guid orderId, Guid articleId)
        {
            await using var context = new DbContextFactory().CreateDbContext();
            var orderLineService = new OrderLineService(context);

            var articleIds = new List<Guid> {articleId};

            var orderLineResult = await orderLineService.RentAsync(orderId, articleIds);
            
            Console.WriteLine($"orderLineResult (True): {orderLineResult.ToString()}");
        }
    }
}
