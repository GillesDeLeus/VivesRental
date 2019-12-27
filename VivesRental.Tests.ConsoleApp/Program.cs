using System;
using Microsoft.EntityFrameworkCore;
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
            TestEdit();
            Console.ReadLine();
        }

        static void TestEdit()
        {
            using var dbContext = new DbContextFactory().CreateDbContext();

            var productRepository = new ProductRepository(dbContext);
            var articleRepository = new ArticleRepository(dbContext);
            var orderRepository = new OrderRepository(dbContext);
            var orderLineRepository = new OrderLineRepository(dbContext);
            var customerRepository = new CustomerRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, productRepository, articleRepository, orderRepository, orderLineRepository, customerRepository);
            
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

            createdArticle.Status = ArticleStatus.Broken;

            var editedArticle = articleService.Edit(createdArticle);

            productService.Remove(createdProduct.Id);
        }

        static void TestRemove()
        {

        }
    }
}
