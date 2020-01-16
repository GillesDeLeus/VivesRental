using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IProductService
    {
        ProductResult Get(Guid id);
        IList<ProductResult> All();
        IList<ProductResult> All(DateTime fromDateTime, DateTime untilDateTime);
        ProductResult Create(Product entity);
        ProductResult Edit(Product entity);
        bool Remove(Guid id);
        bool GenerateArticles(Guid productId, int amount);
        IList<ProductResult> GetAvailableProductResults();
        IList<ProductResult> GetAvailableProductResults(DateTime fromDateTime, DateTime untilDateTime);

    }
}
