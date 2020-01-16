using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
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
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ProductResult Get(Guid id)
        {
            return _unitOfWork.Products
                .Find(p => p.Id == id)
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
            return _unitOfWork.Products
                .Find()
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

            _unitOfWork.Products.Add(product);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
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
            var product = _unitOfWork.Products
                .Find(p => p.Id == entity.Id)
                .FirstOrDefault();

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

            var numberOfObjectsUpdated = _unitOfWork.Complete();
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
            var product = _unitOfWork.Products
                .Find(p => p.Id == id)
                .FirstOrDefault();

            if (product == null)
                return false;

            _unitOfWork.BeginTransaction();
            _unitOfWork.OrderLines.ClearArticleByProductId(id);
            _unitOfWork.ArticleReservations.RemoveByProductId(id);
            _unitOfWork.Articles.RemoveByProductId(id);

            //Remove product
            _unitOfWork.Products.Remove(product.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
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

            _unitOfWork.BeginTransaction();

            for (int i = 0; i < amount; i++)
            {
                var article = new Article
                {
                    ProductId = productId
                };
                _unitOfWork.Articles.Add(article);
            }

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
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
            return _unitOfWork.Products
                .Find(ProductExtensions.IsAvailable(fromDateTime, untilDateTime)) //Only articles that are not reserved in this period
                .MapToResults(fromDateTime, untilDateTime)
                .ToList();
        }

    }
}
