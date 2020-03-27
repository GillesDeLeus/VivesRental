using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Results;

namespace VivesRental.Services
{
    public class ProductService : IProductService
    {
        private readonly IVivesRentalDbContext _context;
        public ProductService(IVivesRentalDbContext context)
        {
            _context = context;
        }


        public Task<ProductResult> GetAsync(Guid id)
        {
            return _context.Products
                .Where(p => p.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefaultAsync();
        }



        public Task<List<ProductResult>> AllAsync()
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;
            return AllAsync(fromDateTime, untilDateTime);
        }

        public Task<List<ProductResult>> AllAsync(DateTime fromDateTime, DateTime untilDateTime)
        {
            return _context.Products
                .MapToResults(fromDateTime, untilDateTime)
                .ToListAsync();
        }

        public async Task<ProductResult> CreateAsync(Product entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Add a clean product
            var product = new Product
            {
                Name = entity.Name,
                Description = entity.Description,
                Manufacturer = entity.Manufacturer,
                Publisher = entity.Publisher,
                RentalExpiresAfterDays = entity.RentalExpiresAfterDays
            };

            _context.Products.Add(product);
            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            if (numberOfObjectsUpdated > 0)
            {
                return await GetAsync(product.Id);
            }
            return null;
        }

        public async Task<ProductResult> EditAsync(Product entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Product from unitOfWork
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == entity.Id);

            if (product == null)
            {
                return null;
            }

            //Only update the properties we want to update
            product.Name = entity.Name;
            product.Description = entity.Description;
            product.Manufacturer = entity.Manufacturer;
            product.Publisher = entity.Publisher;
            product.RentalExpiresAfterDays = entity.RentalExpiresAfterDays;

            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            if (numberOfObjectsUpdated > 0)
            {
                return await GetAsync(product.Id);
            }
            return null;
        }

        /// <summary>
        /// Removes one Product, removes ArticleReservations, removes all linked articles and disconnects OrderLines from articles
        /// </summary>
        /// <param name="id">The id of the Product</param>
        /// <returns>True if the product was deleted</returns>
        public async Task<bool> RemoveAsync(Guid id)
        {
            if (_context.Database.IsInMemory())
            {
                await ClearArticleByProductIdAsync(id);
                _context.ArticleReservations.RemoveRange(
                    _context.ArticleReservations.Where(a => a.Article.ProductId == id));
                _context.Articles.RemoveRange(_context.Articles.Where(a => a.ProductId == id));

                //Remove product
                _context.Products.Remove(id);
                return true;
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await ClearArticleByProductIdAsync(id);
                _context.ArticleReservations.RemoveRange(
                    _context.ArticleReservations.Where(a => a.Article.ProductId == id));
                _context.Articles.RemoveRange(_context.Articles.Where(a => a.ProductId == id));

                //Remove product
                _context.Products.Remove(id);

                var numberOfObjectsUpdated = await _context.SaveChangesWithConcurrencyIgnoreAsync();
                await transaction.CommitAsync();
                return numberOfObjectsUpdated > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Adds a number of articles with Normal status to the Product.
        /// This is limited to maximum 10.000
        /// </summary>
        /// <returns>True if articles are added</returns>
        public async Task<bool> GenerateArticlesAsync(Guid productId, int amount)
        {
            if (amount <= 0 && amount > 10.000) //Set a limit
                return false;

            for (int i = 0; i < amount; i++)
            {
                var article = new Article
                {
                    ProductId = productId
                };
                _context.Articles.Add(article);
            }

            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            return numberOfObjectsUpdated > 0;
        }

        /// <summary>
        /// Retrieves a list of products that are available from now
        /// </summary>
        /// <returns>A list of ProductResults</returns>
        public Task<List<ProductResult>> GetAvailableProductResultsAsync()
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;
            return GetAvailableProductResultsAsync(fromDateTime, untilDateTime);
        }

        /// <summary>
        /// Retrieves a list of products that are available between a from and until date
        /// </summary>
        /// <returns>A list of ProductResults</returns>
        public Task<List<ProductResult>> GetAvailableProductResultsAsync(DateTime fromDateTime, DateTime untilDateTime)
        {
            return _context.Products
                .Where(ProductExtensions.IsAvailable(fromDateTime, untilDateTime)) //Only articles that are not reserved in this period
                .MapToResults(fromDateTime, untilDateTime)
                .ToListAsync();
        }

        private async Task ClearArticleByProductIdAsync(Guid productId)
        {
            if (_context.Database.IsInMemory())
            {
                var orderLines = await _context.OrderLines
                    .Where(ol => ol.Article.ProductId == productId)
                    .ToListAsync();
                foreach (var orderLine in orderLines)
                {
                    orderLine.Article = null;
                    orderLine.ArticleId = null;
                }

                return;
            }

            var commandText =
                "UPDATE OrderLine SET ArticleId = null from OrderLine inner join Article on Article.ProductId = @ProductId";
            var articleIdParameter = new SqlParameter("@ProductId", productId);

            await _context.Database.ExecuteSqlRawAsync(commandText, articleIdParameter);
        }
    }
}
