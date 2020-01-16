using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
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
            return Get(id, null);
        }

        public Product Get(Guid id, ProductIncludes includes)
        {
            return _unitOfWork.Products.Get(id, includes);
        }

        public IList<Product> All()
        {
            return All(null);
        }

        public IList<ProductResult> AllResult()
        {
            return AllResult(null);
        }

        public IList<Product> All(ProductIncludes includes)
        {
            return _unitOfWork.Products.GetAll(includes).ToList();
        }

        public IList<ProductResult> AllResult(ProductIncludes includes)
        {
            return _unitOfWork.Products.GetAllResult(includes).ToList();
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

        public bool Remove(Guid id)
        {
            var product = _unitOfWork.Products.Get(id, new ProductIncludes { ArticleOrderLines = true });
            if (product == null)
                return false;

            //Remove articles
            foreach (var article in product.Articles.ToList())
            {
                //Remove orderLines
                foreach (var orderLine in article.OrderLines.ToList())
                {
                    orderLine.Article = null;
                    orderLine.ArticleId = null;
                }
                _unitOfWork.Articles.Remove(article.Id);
            }
            //Remove product
            _unitOfWork.Products.Remove(product.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        public bool GenerateArticles(Guid productId, int amount)
        {
            if (amount <= 0)
                return false;

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

        public IList<ProductResult> GetAvailableProductResults(ProductIncludes includes = null)
        {
            return _unitOfWork.Products
                .FindResult(p => p.Articles.Any(a => a.Status == ArticleStatus.Normal &&
                                                   a.OrderLines.All(ol => ol.ReturnedAt.HasValue)), includes)
                .ToList();
        }

    }
}
