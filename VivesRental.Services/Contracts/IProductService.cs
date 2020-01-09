using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Repository.Results;

namespace VivesRental.Services.Contracts
{
    public interface IProductService
    {
        Product Get(Guid id, ProductIncludes includes = null);
        IList<Product> All(ProductIncludes includes = null);
        IList<ProductResult> AllResult(ProductIncludes includes = null);
        IList<ProductResult> AllResult(DateTime fromDateTime, DateTime untilDateTime, ProductIncludes includes);
        Product Create(Product entity);
        Product Edit(Product entity);
        bool Remove(Guid id);
        bool GenerateArticles(Guid productId, int amount);
        IList<ProductResult> GetAvailableProductResults();
        IList<ProductResult> GetAvailableProductResults(DateTime fromDateTime, DateTime untilDateTime);

    }
}
