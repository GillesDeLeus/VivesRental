using System;
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
            TestRemove2();
            Console.ReadLine();
        }

        static void TestEdit()
        {
            using var context = new DbContextFactory().CreateDbContext();
            var unitOfWork = CreateUnitOfWork(context);

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
            var orderRepository = new OrderRepository(context);
            var orderLineRepository = new OrderLineRepository(context);
            var customerRepository = new CustomerRepository(context);
            return new UnitOfWork(context, productRepository, articleRepository, orderRepository, orderLineRepository, customerRepository);
        }
    }
}
