using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Repository.Results;

namespace VivesRental.Services.Contracts
{
    public interface IProductService
    {
        Product Get(Guid id);
        Product Get(Guid id, ProductIncludes includes);
        IList<Product> All();
        IList<ProductResult> AllResult();
        IList<Product> All(ProductIncludes includes);
        IList<ProductResult> AllResult(ProductIncludes includes);
        Product Create(Product entity);
        Product Edit(Product entity);
        bool Remove(Guid id);
        bool GenerateArticles(Guid productId, int amount);
        IList<ProductResult> GetAvailableProductResults(ProductIncludes includes = null);

    }
}
