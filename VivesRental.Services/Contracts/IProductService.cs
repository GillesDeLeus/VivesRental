using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.Services.Contracts
{
    public interface IProductService
    {
        Product Get(Guid id);
        IList<Product> All();
        IList<ProductResult> AllResult();
        IList<ProductResult> AllResult(DateTime fromDateTime, DateTime untilDateTime);
        Product Create(Product entity);
        Product Edit(Product entity);
        bool Remove(Guid id);
        bool GenerateArticles(Guid productId, int amount);
        IList<ProductResult> GetAvailableProductResults();
        IList<ProductResult> GetAvailableProductResults(DateTime fromDateTime, DateTime untilDateTime);

    }
}
