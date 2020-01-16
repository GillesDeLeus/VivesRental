using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Results;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;

namespace VivesRental.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Product Get(Guid id)
        {
            return _unitOfWork.Products.Get(id);
        }


        public IList<Product> All()
        {
            return _unitOfWork.Products.GetAll().ToList();
        }

        public IList<ProductResult> AllResult()
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;
            return AllResult(fromDateTime, untilDateTime);
        }

        public IList<ProductResult> AllResult(DateTime fromDateTime, DateTime untilDateTime)
        {
            return _unitOfWork.Products.GetAllResult(fromDateTime, untilDateTime).ToList();
        }

        public Product Create(Product entity)
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
                return product;
            }
            return null;
        }

        public Product Edit(Product entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Product from unitOfWork
            var product = _unitOfWork.Products.Get(entity.Id);
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
                return entity;
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
            var product = _unitOfWork.Products.Get(id);
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
                .FindResult(fromDateTime, untilDateTime) //Only articles that are not reserved in this period
                .ToList();
        }

    }
}
