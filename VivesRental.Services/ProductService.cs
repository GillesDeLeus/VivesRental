using System;
using System.Collections.Generic;
using System.Linq;
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


        public ProductResult Get(Guid id)
        {
            return _context.Products
                .Where(p => p.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();
        }



        public IList<ProductResult> All()
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;
            return All(fromDateTime, untilDateTime);
        }

        public IList<ProductResult> All(DateTime fromDateTime, DateTime untilDateTime)
        {
            return _context.Products
                .MapToResults(fromDateTime, untilDateTime)
                .ToList();
        }

        public ProductResult Create(Product entity)
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
            var numberOfObjectsUpdated = _context.SaveChanges();
            if (numberOfObjectsUpdated > 0)
            {
                return product.MapToResult(DateTime.Now, DateTime.MaxValue);
            }
            return null;
        }

        public ProductResult Edit(Product entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Product from unitOfWork
            var product = _context.Products
                .FirstOrDefault(p => p.Id == entity.Id);

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

            var numberOfObjectsUpdated = _context.SaveChanges();
            if (numberOfObjectsUpdated > 0)
            {
                return entity.MapToResult(DateTime.Now, DateTime.MaxValue);
            }
            return null;
        }

        /// <summary>
        /// Removes one Product, removes ArticleReservations, removes all linked articles and disconnects OrderLines from articles
        /// </summary>
        /// <param name="id">The id of the Product</param>
        /// <returns>True if the product was deleted</returns>
        public bool Remove(Guid id)
        {
            var result = _context.RunInTransaction(() =>
            {
                ClearArticleByProductId(id);
                _context.ArticleReservations.RemoveRange(
                    _context.ArticleReservations.Where(a => a.Article.ProductId == id));
                _context.Articles.RemoveRange(_context.Articles.Where(a => a.ProductId == id));

                //Remove product
                _context.Products.Remove(id);

                var numberOfObjectsUpdated = _context.SaveChangesWithConcurrencyIgnore();

                return numberOfObjectsUpdated > 0;
            });
            return result;
        }

        /// <summary>
        /// Adds a number of articles with Normal status to the Product.
        /// This is limited to maximum 10.000
        /// </summary>
        /// <returns>True if articles are added</returns>
        public bool GenerateArticles(Guid productId, int amount)
        {
            if (amount <= 0 && amount > 10.000) //Set a limit
                return false;

            var result = _context.RunInTransaction(() =>
            {
                for (int i = 0; i < amount; i++)
                {
                    var article = new Article
                    {
                        ProductId = productId
                    };
                    _context.Articles.Add(article);
                }

                var numberOfObjectsUpdated = _context.SaveChanges();
                return numberOfObjectsUpdated > 0;
            });
            return result;
        }

        /// <summary>
        /// Retrieves a list of products that are available from now
        /// </summary>
        /// <returns>A list of ProductResults</returns>
        public IList<ProductResult> GetAvailableProductResults()
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;
            return GetAvailableProductResults(fromDateTime, untilDateTime);
        }

        /// <summary>
        /// Retrieves a list of products that are available between a from and until date
        /// </summary>
        /// <returns>A list of ProductResults</returns>
        public IList<ProductResult> GetAvailableProductResults(DateTime fromDateTime, DateTime untilDateTime)
        {
            return _context.Products
                .Where(ProductExtensions.IsAvailable(fromDateTime, untilDateTime)) //Only articles that are not reserved in this period
                .MapToResults(fromDateTime, untilDateTime)
                .ToList();
        }

        private void ClearArticleByProductId(Guid productId)
        {
            if (_context.Database.IsInMemory())
            {
                var orderLines = _context.OrderLines.Where(ol => ol.Article.ProductId == productId).ToList();
                foreach (var orderLine in orderLines)
                {
                    orderLine.Article = null;
                    orderLine.ArticleId = null;
                }
                return;
            }

            var commandText = "UPDATE OrderLine SET ArticleId = null from OrderLine inner join Article on Article.ProductId = @ProductId";
            var articleIdParameter = new SqlParameter("@ProductId", productId);

            _context.Database.ExecuteSqlRaw(commandText, articleIdParameter);
        }

    }
}
